        /// <summary>
        /// Merge two IEnumerable together. Returning a tuples of matched items from both IEnumerable.
        /// </summary>
        public static IEnumerable<(TKey Key, TLeft LeftItem, TRight RightItem)> MergeEnumerable<TKey, TLeft, TRight>(
            this IComparer<TKey> keyComparer,
            IEnumerable<TLeft> leftEnumerable,
            IEnumerable<TRight> rightEnumerable,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector)
            where TLeft : class
            where TRight : class
        {
            using var leftEnumerator = leftEnumerable.OrderBy(leftKeySelector, keyComparer).GetEnumerator();
            using var rightEnumerator = rightEnumerable.OrderBy(rightKeySelector, keyComparer).GetEnumerator();

            var leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
            var rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;

            while (leftItem is not null || rightItem is not null)
            {
                // Fast forward leftEnumerator if necessary
                while (leftItem is not null && (rightItem is null || 0 > keyComparer.Compare(leftKeySelector(leftItem), rightKeySelector(rightItem))))
                {
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                }

                // Fast forward rightEnumerator if necessary
                while (rightItem is not null && (leftItem is null || 0 < keyComparer.Compare(leftKeySelector(leftItem), rightKeySelector(rightItem))))
                {
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                }

                // Match between left and right
                if (leftItem is not null && rightItem is not null && 0 == keyComparer.Compare(leftKeySelector(leftItem), rightKeySelector(rightItem)))
                {
                    yield return (leftKeySelector(leftItem), leftItem, rightItem);
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                }
            }
        }
