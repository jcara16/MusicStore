using System.Collections.Generic;
using MusicStoree.Models;

namespace MusicStoree.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}  