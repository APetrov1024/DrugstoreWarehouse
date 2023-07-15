using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace DrugstoreWarehouse.Pages;

public class Index_Tests : DrugstoreWarehouseWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
