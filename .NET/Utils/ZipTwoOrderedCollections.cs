        /// <summary>
        /// Exemple usage
        /// </summary>
        private IEnumerable<PriceData> GetPriceDataEnumerable(IEnumerable<AxProductPrice> axData, ICollection<ProductCorrespondence> correspondencesCollection)
        {
            var comparer = Comparer<string>.Create(StringComparer.OrdinalIgnoreCase.Compare);
            return Zip(axData, correspondencesCollection, x => x.AxItemId, x => x.ExternalId, comparer)
                    .Select(x => new PriceData(x.right.ProductId, x.left.Msrp, x.left.SalesPrice));
        }

        /// <summary>
        /// Zip two ordered list
        /// </summary>
        public IEnumerable<(TL left, TR right)> Zip<TL, TR , TLKey>(
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

            while (leftItem is not null && rightItem is not null)
            {
                // Fast forward leftEnumerator if necessary
                while (leftItem is not null && 0 > comparer.Compare(leftKey(leftItem), rightKey(rightItem)))
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                if (leftItem is null)
                    break;

                // Fast forward rightEnumerator if necessary
                while (rightItem is not null && 0 < comparer.Compare(leftKey(leftItem), rightKey(rightItem)))
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                if (rightItem is null)
                    break;

                // Match between left and right
                if (0 == comparer.Compare(leftKey(leftItem), rightKey(rightItem)))
                {
                    yield return (leftItem, rightItem);
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                }
            }
        }
