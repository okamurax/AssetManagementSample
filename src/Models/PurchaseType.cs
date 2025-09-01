using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models
{
    public class SelectItems
    {
        public IEnumerable<SelectListItem> AcquiredType { get; } = new List<SelectListItem>
        {
            // Edit/所有形態
            new SelectListItem {Text ="購入品", Value = "購入品"},
            new SelectListItem {Text ="レンタル品", Value = "レンタル品"},
            new SelectListItem {Text ="リース品(所有権移転)", Value = "リース品(所有権移転)"}
        };
    }
}
