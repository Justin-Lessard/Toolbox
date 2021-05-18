        /// <summary>
        /// Zip two list
        /// </summary>
        private IEnumerable<(TLKey key, TL? left, TR? right)> Zip<TL, TR, TLKey>(
            IEnumerable<TL> leftEnumerable,
            IEnumerable<TR> rightEnumerable,
            Func<TL, TLKey> leftKey,
            Func<TR, TLKey> rightKey,
            IComparer<TLKey> comparer)
            where TL : class
            where TR : class
        {
            using var leftEnumerator = leftEnumerable.OrderBy(leftKey).GetEnumerator();
            using var rightEnumerator = rightEnumerable.OrderBy(rightKey).GetEnumerator();

            var leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
            var rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;

            while (leftItem is not null || rightItem is not null)
            {
                // Fast forward leftEnumerator if necessary
                while (leftItem is not null && (rightItem is null || 0 > comparer.Compare(leftKey(leftItem), rightKey(rightItem))))
                {
                    yield return (leftKey(leftItem), leftItem, null);
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                }

                // Fast forward rightEnumerator if necessary
                while (rightItem is not null && (leftItem is null || 0 < comparer.Compare(leftKey(leftItem), rightKey(rightItem))))
                {
                    yield return (rightKey(rightItem), null, rightItem);
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                }

                // Match between left and right
                if (leftItem is not null && rightItem is not null && 0 == comparer.Compare(leftKey(leftItem), rightKey(rightItem)))
                {
                    yield return (leftKey(leftItem), leftItem, rightItem);
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                }
            }
