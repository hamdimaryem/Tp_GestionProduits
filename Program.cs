using System.Globalization;
using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.EntityFrameworkCore;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // Catégories : ajoute celles qui manquent
    var nomsCategories = new[] { "Informatique", "Maison", "Vêtements", "Électronique", "Sport", "Alimentation", "Papeterie" };
    foreach (var nom in nomsCategories)
    {
        if (!db.Categories.Any(c => c.Nom == nom))
            db.Categories.Add(new Categorie { Nom = nom });
    }
    db.SaveChanges();

    // Produits : ajoute ceux qui n'existent pas encore (pas de doublons)
    var cats = db.Categories.ToDictionary(c => c.Nom, c => c.Id);
    var produits = new (string Nom, decimal Prix, int Stock, string Cat)[]
    {
        ("Clavier mécanique", 189.900m, 25, "Informatique"),
        ("Souris sans fil", 59.500m, 40, "Informatique"),
        ("Écran 24 pouces", 649.000m, 4, "Informatique"),
        ("Disque SSD 1 To", 279.000m, 0, "Informatique"),
        ("Casque Bluetooth", 149.000m, 12, "Électronique"),
        ("Enceinte portable", 99.900m, 3, "Électronique"),
        ("Smartphone 128 Go", 1299.000m, 8, "Électronique"),
        ("Lampe de bureau LED", 45.000m, 30, "Maison"),
        ("Set de casseroles", 159.500m, 0, "Maison"),
        ("Aspirateur robot", 899.000m, 5, "Maison"),
        ("T-shirt coton", 29.900m, 80, "Vêtements"),
        ("Jean slim", 89.000m, 35, "Vêtements"),
        ("Veste d'hiver", 199.000m, 2, "Vêtements"),
        ("Ballon de football", 49.000m, 60, "Sport"),
        ("Tapis de yoga", 39.500m, 18, "Sport"),
        ("Haltères 10 kg", 85.000m, 0, "Sport"),
        ("Huile d'olive 1 L", 24.000m, 120, "Alimentation"),
        ("Café moulu 500 g", 18.500m, 45, "Alimentation"),
        ("Cahier A4 96 pages", 3.500m, 200, "Papeterie"),
        ("Stylos (lot de 10)", 7.900m, 4, "Papeterie")
    };

    foreach (var p in produits)
    {
        if (!db.Produits.Any(x => x.Nom == p.Nom))
            db.Produits.Add(new Produit { Nom = p.Nom, Prix = p.Prix, Stock = p.Stock, CategorieId = cats[p.Cat] });
    }
    db.SaveChanges();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/Produits"));
app.MapRazorPages();

app.Run();