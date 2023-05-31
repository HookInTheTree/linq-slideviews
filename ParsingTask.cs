using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
	/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
	/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
	public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
	{
		return lines.Skip(1).Select(x =>
			{
				var propertyValues = x.Split(';');
				try
				{
					var slideId = int.Parse(propertyValues[0]);
					
					if (!Enum.TryParse(propertyValues[1], true, out SlideType slideType))
						throw new FormatException();
					
					var unitTitle = propertyValues[2];
					return new SlideRecord(slideId, slideType, unitTitle);
				}
				catch (Exception exc)
				{
					return null;
				}
			})
			.Where(x => x != null)
			.ToDictionary(slide => slide.SlideId, slide => slide);
	}

	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
	/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
	/// Такой словарь можно получить методом ParseSlideRecords</param>
	/// <returns>Список информации о посещениях</returns>
	/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
	public static IEnumerable<VisitRecord> ParseVisitRecords(
		IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
	{
		return lines.Skip(1).Select(x =>
		{
			var properties = x.Split(';');

			try
			{
                var userId = int.Parse(properties[0]);
                var slideId = int.Parse(properties[1]);
                var date = DateTime.Parse(properties[2]);
                var time = TimeSpan.Parse(properties[3]);

                var dateTime = date.Add(time);
                var slide = slides[slideId];
                return new VisitRecord(userId, slide.SlideId, dateTime, slide.SlideType);
            }
			catch(Exception exc)
			{
				throw new FormatException($"Wrong line [{x}]");
			}
		});
	}
}