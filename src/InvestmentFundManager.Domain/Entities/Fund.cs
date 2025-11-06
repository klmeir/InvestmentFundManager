namespace InvestmentFundManager.Domain.Entities
{
    // <summary>
    /// Represents an investment fund within the domain.
    /// </summary>
    public class Fund
    {
        /// <summary>
        /// Unique identifier of the fund.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Name of the fund.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Minimum amount required to subscribe to the fund.
        /// </summary>
        public decimal MinimumAmount { get; private set; }

        /// <summary>
        /// Fund category (e.g., FPV or FIC).
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Constructor for creating a Fund entity.
        /// </summary>
        /// <param name="id">Unique identifier of the fund.</param>
        /// <param name="name">Name of the fund.</param>
        /// <param name="minimumAmount">Minimum subscription amount.</param>
        /// <param name="category">Fund category.</param>
        public Fund(string id, string name, decimal minimumAmount, string category)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MinimumAmount = minimumAmount;
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }
    }
}
