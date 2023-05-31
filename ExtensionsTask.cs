using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
		var sequense = items.ToArray();

		var sorted = sequense.OrderBy(x => x).ToArray();
		var length = sorted.Length;

		if (length == 0)
			throw new InvalidOperationException();

		var index = (int) length / 2;
        double median = default(double);
		if (length % 2 == 0)
			median = sorted.Skip(index - 1).Take(2).Sum() / 2;
		else
            median = sorted.Skip(index).Take(1).FirstOrDefault();
        
		return median;
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
		var enumerator = items.GetEnumerator();
		enumerator.MoveNext();
        var first = enumerator.Current;
		var second = default(T);

        while (enumerator.MoveNext())
		{
			second = enumerator.Current;
            yield return (first, second);
			first = second;
		}
	}
}