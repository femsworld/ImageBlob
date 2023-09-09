using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageBlob.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Configure BlobStorageOptions
builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection("BlobStorage"));

// Add services to the container.
builder.Services.AddRazorPages();

// Register BlobServiceClient
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage")));

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// app.MapFallbackToPage("/Upload");

app.Run();
