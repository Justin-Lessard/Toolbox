var comparator = (l,r) => StringComparer.OrdinalIgnoreCase.Compare(l, r);
using var leftEnumerator = someCollection.OrderBy(x => x.Key).GetEnumerator();
using var rightEnumerator = otherCollection.OrderBy(x => x.Key).GetEnumerator();

var leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
var rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;

while (true)
{
	// Fast forward leftEnumerator if necessary
	while (leftItem is not null && 0 > comparator(leftItem?.Key, rightItem?.Key)
		leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;

	// Fast forward rightEnumerator if necessary
	while (rightItem is not null && 0 < comparator(leftItem?.Key, rightItem?.Key)
		rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;

	// Match between left and right
	if (leftItem is not null && rightItem is not null && 0 == comparator(leftItem.Key, rightItem?.Key))
	{
		yield return (leftItem, rightItem);
		
		leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
		rightItem = otherList.MoveNext() ? otherList.Current : null;
		continue;
	}
	
	if (leftItem is null || rightItem is null)
		break;
}
