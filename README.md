### DynamicFilter
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
		"name": "where",
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
		"name": "distinct"
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
> Such implementation of Select was made due to the limitations of anonymous types and should be used as the last operation 
> and only to reduce the final number of properties to be fetched from the database.

At this point following operators are available to use in Where predicate:
```
    Equals
    NotEqual
    Any
    Greater
    GreaterOrEqual
    Less
    LessOrEqual
    Exists
    NotExists
    StartsWith
    EndsWith
    Contains
    NotContains
```
## Examples

#### Advanced filtering
```c#
.Where(x => ((x.Name.EndsWith("Mars") || ["Snickers", "Mars"].Contains(x.Name)) && x.ExpireDate >= DateTime.UtcNow) && (x.IsForSale || x.IsInStock))
```
```json
[
	{
		"name": "where",
		"arguments": 
		{
			"conditions": 
			[
				{
					"field": "Name",
					"operator": "EndsWith",
					"value": ["Mars"]
				},
				{
					"logic": "Or",
					"field": "Name",
					"operator": "Any",
					"value": ["Snickers", "Mars"]
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

#### Fetching specific field values
```c#
.Select(x => x.Price)
.Distinct()
.OrderBy(x => x)
```
```json
[
	{
		"name": "select",
		"arguments": "Price"
	},
	{
		"name": "distinct"
	},
	{
		"name": "orderBy"
	}
]
```
