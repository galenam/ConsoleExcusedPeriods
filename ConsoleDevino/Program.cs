using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriodsTest
{
    internal static class Program
    {
        private static IEnumerable<Period> GetBookedExcuses()
        {
			/*
			 * Тесты не делаю, увеличиваю обьем исходных данных. Считаю, что периоды 0 длины, попадающие в интервал допустимого времени, должны 
			 * выводиться
			 */
			return new List<Period>{ 
			new Period(10,11),
            new Period(8,19),
            new Period(8,10),
            new Period(15,20),
            new Period(20,21),
            new Period(7,8)};
        }

        private static void Main()
        {
            var workTime = new Period(9,18);

            var bookedExcuses = GetBookedExcuses();

            Console.WriteLine(workTime);
            Console.WriteLine();

            foreach (var excusedPeriod in FindExcusedPeriods(workTime, bookedExcuses))
            {
                Console.WriteLine(excusedPeriod);
            }

            Console.Read();

            // Для рабочего времени 09:00-18:00 должно выводить:
            // > 09:00 - 18:00
            // >
            // > 10:00 - 11:00
            // > 09:00 - 18:00
            // > 09:00 - 10:00
            // > 15:00 - 18:00
        }

        private static IEnumerable<Period> FindExcusedPeriods(PeriodBase workTime, IEnumerable<Period> bookedExcuses)
        {
            return  bookedExcuses.Where(p => p.End >= workTime.Start && p.Start <= workTime.End).
                Select(p =>
                { 
					var begin = p.Start < workTime.Start ? workTime.Start : p.Start;
                    var end = p.End > workTime.End ? workTime.End : p.End;
                    return new Period(begin, end);
                });
        }

        private abstract class PeriodBase
        {
            internal TimeSpan Start { get;}

            internal TimeSpan End { get; }

	        protected PeriodBase(TimeSpan start, TimeSpan end)
            {
				if (start >= end)
				{
					throw new ArgumentException("Начало периода должно быть строго меньше конца периода.", nameof(start));
				}

				if (IsRestricted(start))
	            {
					throw new ArgumentException("Не укладывается в сутки.", nameof(start));
				}

				if (IsRestricted(end))
				{
					throw new ArgumentException("Не укладывается в сутки.", nameof(end));
				}

				Start = start;
                End = end;
			}

	        private static bool IsRestricted(TimeSpan t)
	        {
		        return t > new TimeSpan(23, 59, 50) || t < TimeSpan.Zero;

	        }

	        protected PeriodBase(int beginHour, int endHour):this(new TimeSpan(beginHour, 0,0), new TimeSpan(endHour,0,0))
	        {
			}

			public override string ToString()
            {
                return Start + " " + End;
            }
        }

        private sealed class Period : PeriodBase
        {
            internal Period(TimeSpan start, TimeSpan end) : base(start, end){}

			internal Period(int beginHour, int endHour):base(beginHour, endHour) { }
        }

    }
}
