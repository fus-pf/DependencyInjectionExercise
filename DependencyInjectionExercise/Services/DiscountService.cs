namespace DependencyInjectionExercise.Services;

public class DiscountService
{
    private Dictionary<string, int> _orderCountInSession = new Dictionary<string, int>();
    private string _currentCustomerName = string.Empty;

    public decimal LastAppliedDiscount { get; private set; }
    public string LastCustomerName => _currentCustomerName;
    public int OrderCountInSession => _orderCountInSession.TryGetValue(_currentCustomerName, out var value) ? value : 0;

    public decimal CalculateDiscount(string category, int quantity, string customerName)
    {
        _currentCustomerName = customerName;

        if (_orderCountInSession.ContainsKey(customerName))
        {
            _orderCountInSession[customerName]++;
        }
        else
        {
            _orderCountInSession[customerName] = 1;
        }

        decimal discountPercent = 0;

        if (category == "fiction" && quantity >= 5)
        {
            discountPercent = 0.10m;
        }
        else if (category == "non-fiction" && quantity >= 3)
        {
            discountPercent = 0.15m;
        }

        // Loyalty bonus: every 3rd order gets an extra 5% off
        if (_orderCountInSession[customerName] % 3 == 0)
        {
            discountPercent += 0.05m;
        }

        LastAppliedDiscount = discountPercent;

        return discountPercent;
    }
}
