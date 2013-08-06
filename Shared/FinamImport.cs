using System.Collections;
using System;
using System.Net;
using System.IO;
using System.Threading;
using System.Xml;

namespace FinamImport
{
	public class Loader
	{
		public struct Slice
		{			
			public DateTime datetime;			
			public double open;
			public double high;
			public double low;
			public double close;
			public double volume;
		}

		private const string _host = "195.128.78.52";

		// формат даты 
		// 1 - YYYYMMDD
		// 2 - YYMMDD
		// 3 - DDMMYY
		// 4 - DD/MM/YY
		// 5 - MM/DD/YY
		private const string _slice_date_format = "1";

		//  формат времени свеч
		// 1 - HHMMSS
		// 2 - HHMM
		// 3 - HH:MM:SS
		// 4 - HH:MM
		private const string _slice_time_format = "4";

		//  дата начала(0) или окончания(1) свечи
		private const string _slice_begin_end = "0";

		//  разделитель полей
		// 1 - запятая (,)
		// 2 - точка (.)
		// 3 - точка с запятой (;)
		// 4 - табуляция
		// 5 - пробел
		private const string _field_separator = "3";

		//  разделитель полей (сам символ)
		private const char _field_sep_char = ';';

		//  разделитель разрядов
		// 1 - нет
		// 2 - точка (.)
		// 3 - запятая (,)
		// 4 - пробел
		// 5 - кавычка (')
		private const string _num_separator = "1";

		//  формат записи в файл
		// ...
		// 5 - DATE, TIME, OPEN, MAX, MIN, CLOSE, VOLUME
		private const string _fields = "5";

		private const string _referer = "http://www.finam.ru/analysis/export/default.asp";
	
		// список ID импортируемых с финама классов (class_id) 
		// 1  - ММВБ Акции
		// 2  - ММВБ облигации
		// 3  - РТС
		// 4  - МФБ
		// 5  - Валюты
		// 6  - Индексы
		// 7  - Фьючерсы
		// 8  - Российские акции на международных рынках (АДР)
		// 9  - СПФБ
		// 10 - РТС-GAZ
		// 11 - РТС-GTS
		// 12 - ММВБ Внесписочные облигации
		// 14 - Фьючерсы ФОРТС
		// 16 - ММВБ Архив (не торгуемые)
		// 17 - Фьючерсы ФОРТС (архив)
		// 18 - РТС Архив (не торгуемые)
		// 20 - РТС-BOARD
		// 24 - Сырье
		// 29 - ММВБ ПИФы

		//  какой	 интервал свеч брать
		// 1 - Тики
		// 2 - 1 мин.
		// 3 - 5 мин.
		// 4 - 10 мин.
		// 5 - 15 мин.
		// 6 - 30 мин.
		// 7 - час
		// 11 - час (с 10:30)
		// 8 - день
		// 9 - неделя
		// 10 - месяц


		public static uint[] getMinuteIntervals()
		{
			return new uint[] { 1, 2, 3, 5, 15, 30, 60, 120, 240, 720, 1440 };
		}

		protected Hashtable _slices_cache;
		protected string _cache_mark;

		public struct Instrument
		{
			public string class_id;
			public string id;
			public string name;
		}

		protected Queue _instruments;

		public Loader()
		{
			_cache_mark = "";

			_slices_cache = new Hashtable();

            XmlDocument doc = new XmlDocument();

            try {
                doc.Load("config.xml");

                XmlNodeList instruments = doc.DocumentElement.ChildNodes;

                _instruments = new Queue();

                foreach (XmlNode i in instruments) {
                    Instrument instrument = new Instrument();

			        instrument.class_id = i.Attributes["class_id"].Value;
			        instrument.id = i.Attributes["id"].Value;
			        instrument.name = i.Attributes["name"].Value;

			        _instruments.Enqueue(instrument);
                }

            }
            catch (Exception ex) {
                // MessageBox.Show("Ошибка: " + ex.Message);
            }
		}

		protected ArrayList getSlices(	string class_id,
										string security_id,									
										string period,
										string date_from,	// YYYY-MM-DD
										string date_to,		// YYYY-MM-DD
										string instrument)
		{			
			string url = "http://" + _host + "/";

			url += instrument + "_" + date_from + "_" + date_to + ".csv";
			url += "?d=d";

			// площадка
			url += "&m=" + class_id;
			// инструмент
			url += "&em=" + security_id;

			url += "&p=" + period;
			url += "&df=" + date_from.Substring(8, 2);
			url += "&mf=" + Convert.ToString(Convert.ToInt16(date_from.Substring(5, 2)) - 1);
			url += "&yf=" + date_from.Substring(0, 4);
			url += "&mt=" + Convert.ToString(Convert.ToInt16(date_to.Substring(5, 2)) - 1);
			url += "&yt=" + date_to.Substring(0, 4);
			url += "&f=" + instrument + ".csv";
			url += "&e=.csv";
			url += "&dtf=" + _slice_date_format;
			url += "&tmf=" + _slice_time_format;
			url += "&MSOR=" + _slice_begin_end;
			url += "&sep=" + _field_separator;
			url += "&sep2=" + _num_separator;
			url += "&datf=" + _fields;

			// вставить заголовок
			url += "&at=0";
			// заполнять периоды без сделок
			//url += "&fsp=0";
			// код бумаги
			//url += "&cn=";

            ArrayList slices = new ArrayList();
            try {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Referer = _referer;

                // Get the response.
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                // Display the status.
                //Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string response_str = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

                string[] lines = response_str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            
                foreach (string line in lines)
                {
                    string[] tokens = line.Replace('.', ',').Split(_field_sep_char);

                    if (tokens.Length != 7)
                        continue;

                    Slice slice = new Slice();

                    slice.datetime = new DateTime
                        (
                            Convert.ToInt32(tokens[0].Substring(0, 4)),	// год
                            Convert.ToInt32(tokens[0].Substring(4, 2)),	// месяц
                            Convert.ToInt32(tokens[0].Substring(6, 2)),	// день
                            Convert.ToInt32(tokens[1].Substring(0, 2)),	// часы
                            Convert.ToInt32(tokens[1].Substring(3, 2)),	// минуты
                            0	// секунды
                        );
                    slice.open = Convert.ToDouble(tokens[2]);
                    slice.high = Convert.ToDouble(tokens[3]);
                    slice.low = Convert.ToDouble(tokens[4]);
                    slice.close = Convert.ToDouble(tokens[5]);
                    slice.volume = Convert.ToDouble(tokens[6]);

                    slices.Add(slice);
                }
            }
            catch {
                // do nothing
            }

			return slices;
		}

		public Queue getData()
		{
			DateTime now = DateTime.Today;

			string month = Convert.ToString(now.Month);
			if(month.Length == 1)
			{
				month = '0' + month;
			}

			string day = Convert.ToString(now.Day);
			if (day.Length == 1)
			{
				day = '0' + day;
			}

			string today = Convert.ToString(now.Year) + '-' + month + '-' + day;

			now = now.AddDays(-1);

			month = Convert.ToString(now.Month);
			if (month.Length == 1)
			{
				month = '0' + month;
			}

			day = Convert.ToString(now.Day);
			if (day.Length == 1)
			{
				day = '0' + day;
			}

			string yesterday = Convert.ToString(now.Year) + '-' + month + '-' + day;

			// проверем, не нужно ли очистить и заполнить заново кеш
			if (_cache_mark != yesterday)
			{
				_slices_cache.Clear();

				_cache_mark = yesterday;
			}

			Queue data = new Queue();

			foreach (Instrument instrument in _instruments)
			{
				try
				{
					ArrayList _slices = new ArrayList();

					if (!_slices_cache.Contains(instrument.name))
					{
						// добавляем значения за вчера в кеш
						_slices_cache.Add(instrument.name, getSlices(instrument.class_id, instrument.id, "2", yesterday, yesterday, instrument.name));
					}

					// достаём значения за вчера из кеша
					_slices.AddRange((ICollection)_slices_cache[instrument.name]);

					// добавляем сегодняшние значения
					_slices.AddRange(getSlices(instrument.class_id, instrument.id, "2", today, today, instrument.name));

					if (_slices.Count > 0)
					{
						ArrayList slices = new ArrayList(_slices);
						slices.Reverse();

						IEnumerator en = (IEnumerator)slices.GetEnumerator();
						en.MoveNext(); // переходим к первому элементу				

						// запоминаем последнее значение
						FinamImport.Loader.Slice slice = (FinamImport.Loader.Slice)en.Current;
						double last_value = slice.close;
						DateTime last_datetime = slice.datetime;

						uint[] minute_intervals = getMinuteIntervals();
						double[] values = new double[minute_intervals.Length];

						for(int index = 0; index < values.Length; index++)
						{
							values[index] = 0;
						}

//						double sum = 0;
//						int count = 0;

						while (en.MoveNext())
						{
							slice = (FinamImport.Loader.Slice)en.Current;

							TimeSpan span = last_datetime - slice.datetime;
							int mins = (int)span.TotalMinutes;
							//sum += slice.open;

							double value = slice.close;//sum / ++count;

							int index = 0;
							for (; index < minute_intervals.Length; index++)
							{
								if (mins <= minute_intervals[index])
									break;
							}

							if (index < minute_intervals.Length)
							{
								values[index] = value;
							}
							else
							{
								// нашли первый из неотслеживаемых интервалов - закончить просмотр
								break;
							}
						}

						Queue row = new Queue();

						row.Enqueue(instrument.name);

						for (int index = 0; index < values.Length; index++)
						{
							row.Enqueue(values[index] != 0 ? String.Format("{0:F3}", ((last_value / values[index] - 1) * 100)) : "");
						}

						data.Enqueue(row);
					}
				}
				catch {
					// пока ничо не делаем c ошибками
				}
			}

			return data;
		}
	}

	public class SharedLoader : MarshalByRefObject
	{
		protected Queue _data;
		protected Loader _loader;
		protected Timer _timer;

		public SharedLoader()
		{
			_loader = new Loader();
			_data = new Queue();

			updateData();

			_timer = new Timer(new TimerCallback(onTimer), null, 30000, 30000);
		}

		private void onTimer(Object state)
		{
			updateData();
		}

		private void logEvent(string str)
		{
			string path = @"c:\IndicesService.log";
			using (StreamWriter sw = File.CreateText(path))
			{
				sw.WriteLine(str);
			}
		}

		private void updateData()
		{
//			logEvent("updating...");
			_data = _loader.getData();
//			logEvent("updated...");
		}

		public Queue getData()
		{
			return _data;
		}
	}
}