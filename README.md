# DynamicFilter
![Nuget](https://img.shields.io/nuget/v/ART4S.DynamicFilter)

DynamicFilter allows to use essential linq filtering methods on frontend side.

All operations were tested against EF Core and PostgreSql.

## Usage

Model:

```c#
class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
    public bool IsForSale { get; set; }
    public DateTime ExpireDate { get; set; }
}
```

Controller:

```c#
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _dbcontext;

    public ProductsController(AppDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    [HttpPost("filter")]
    public IActionResult GetFiltered([FromBody] Operation[] filter)
    {
        return Ok(_dbcontext.Set<Product>().ApplyDynamicFilter(filter));
    }
}
```

Filter:

```json
[
	{
		"name": "Where",
		"arguments": 
                {
			"conditions": 
			[
				{
					"field": "Name",
					"operator": "Contains",
					"value": ["Snickers"]
				},
				{
					"logic": "And",
					"field": "IsForSale",
					"operator": "Equals",
					"value": ["true"]
				}
			],
			"groups": []
		}
	},
	{
		"name": "Distinct",
		"Arguments": {}
	},
	{
		"name": "orderBy",
		"arguments": "ExpireDate"
	},
	{
		"name": "orderByDescending",
		"arguments": "Price"
	},
	{
		"name": "thenBy",
		"arguments": "IsInStock"
	},
	{
		"name": "thenByDescending",
		"arguments": "IsForSale"
	},
        {
		"name": "skip",
		"arguments": 1
	},
	{
		"name": "take",
		"arguments": 2
	},
	{
		"name": "select",
		"arguments": ["Id", "Name"]
	}
]
```

Under the hood filter transforms into call
```c#
_dbcontext.Set<Product>()
    .Where(x => x.Name.Contains("Snickers") && x.IsForSale)
    .Distinct()
    .OrderBy(x => x.ExpireDate)
    .OrderByDescending(x => x.Price)
    .ThenBy(x => x.IsInStock)
    .ThenByDescending(x => x.IsForSale)
    .Skip(1)
    .Take(2)
    .Select(x => new Dictionary<string, object>()
    {
        {"Id", x.Id},
        {"Name", x.Name}
    });
```

## Examples

#### Advanced filtering
```c#
queryable.Where(x => ((x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow) && (x.IsForSale || x.IsInStock))
```
```json
[
	{
		"name": "Where",
		"arguments": 
		{
			"conditions": 
			[
				{
					"field": "Name",
					"operator": "StartsWith",
					"value": ["Snickers"]
				},
				{
					"logic": "Or",
					"field": "Name",
					"operator": "Contains",
					"value": ["Mars"]
				},
				{
					"logic": "And",
					"field": "ExpireDate",
					"operator": "GreaterOrEqual",
					"value": ["2023-02-13 17:56:19Z"]
				},
				{
					"logic": "And",
					"field": "IsForSale",
					"operator": "Equals",
					"value": ["true"]
				},
				{
					"logic": "Or",
					"field": "IsInStock",
					"operator": "Equals",
					"value": ["true"]
				}
			],
			"groups": 
			[
				{
					"start": 1,
					"end": 2,
					"level": 1
				},
				{
					"start": 1,
					"end": 3,
					"level": 2
				},
				{
					"start": 4,
					"end": 5,
					"level": 2
				}
			]
		}
	}
]
```

## Remark

Such implementation of "Select" was made due to the limitations of anonymous types and should be used as the last operation 
and only to reduce the final number of properties to be fetched from the database.

## Operations vs Types compatibility

![Compatibility1](https://user-images.githubusercontent.com/24371700/162436461-09717eaa-23d4-4693-af71-eed40aab02ee.png) 
![Compatibility2](https://user-images.githubusercontent.com/24371700/162436470-3e3db5e0-ab62-4add-bdb1-91664017a4e6.png)
![Compatibility3](https://user-images.githubusercontent.com/24371700/162436496-2d995028-8e68-48f1-8c67-5698792a5527.png)
