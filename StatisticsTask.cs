using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		var usersVisits = visits
			.OrderBy(x => x.DateTime)
			.GroupBy(x => x.UserId)
			.ToList();

		var dateDiffs = usersVisits
			.SelectMany(x => x.Bigrams())
			.Where(x => x.First.SlideType == slideType || x.First.UserId != x.Second.UserId)
			.Select(x => x.Second.DateTime - x.First.DateTime);

		var filteredDateDiffs = dateDiffs
			.Where(x => x.TotalMinutes <= 120 && x.TotalMinutes >= 1)
			.Select(x => x.TotalMinutes);

		if (filteredDateDiffs.Count() == 0)
			return 0;

		var median = filteredDateDiffs.Median();

		return median;
	}
}