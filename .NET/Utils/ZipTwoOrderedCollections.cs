   /// <summary>
        /// Zip two ordered list
        /// </summary>
        public IEnumerable<(TL left, TR right)> Zip<TL, TR>(IEnumerator<TL> leftEnumerator, IEnumerator<TR> rightEnumerator, Func<TL, TR, int> _comparator)
            where TL : class
            where TR : class
        {
            var leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
            var rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;

            while (leftItem is not null && rightItem is not null)
            {
                // Fast forward leftEnumerator if necessary
                while (leftItem is not null && 0 > _comparator(leftItem, rightItem))
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                if (leftItem is null)
                    break;

                // Fast forward rightEnumerator if necessary
                while (rightItem is not null && 0 < _comparator(leftItem, rightItem))
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                if (rightItem is null)
                    break;

                // Match between left and right
                if (0 == _comparator(leftItem, rightItem))
                {
                    yield return (leftItem, rightItem);
                    leftItem = leftEnumerator.MoveNext() ? leftEnumerator.Current : null;
                    rightItem = rightEnumerator.MoveNext() ? rightEnumerator.Current : null;
                }
            }
        }







        /// <summary>
        /// Exemple usage
        /// </summary>
        private IEnumerable<PriceData> GetPriceDataEnumerable(IEnumerable<AxProductPrice> axData, ICollection<ProductCorrespondence> correspondencesCollection)
        {
            int Comparator(AxProductPrice l, ProductCorrespondence r) => StringComparer.OrdinalIgnoreCase.Compare(l.AxItemId, r.ExternalId);

            using var leftEnumerator = axData.OrderBy(x => x.AxItemId).GetEnumerator();
            using var rightEnumerator = correspondencesCollection.OrderBy(x => x.ExternalId).GetEnumerator();

            return Zip(leftEnumerator, rightEnumerator, Comparator)
                .Select(x => new PriceData(x.right.ProductId, x.left.Msrp, x.left.SalesPrice));
        }
