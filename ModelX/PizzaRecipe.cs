﻿using MochaCore.Settings;
using System.Text.Json;

namespace ModelX
{
    public class PizzaRecipe : ISettingsSection
    {
        public PizzaStyle Style { get; set; }

        public bool IsThickCrust { get; set; }

        public FlourType FlourType { get; set; }

        public double Flour { get; set; }

        public double Water { get; set; }

        public double Yeast { get; set; }

        public double Salt { get; set; }

        public List<Topping> Toppings { get; set; } = [];

        public double BakingTemp { get; set; }

        public double Rating { get; set; }

        public string? Notes { get; set; }

        public async Task FillValuesAsync(string serializedData)
        {
            PizzaRecipe? recipe = await Task.Run(() => JsonSerializer.Deserialize<PizzaRecipe>(serializedData));
            if (recipe is not null)
            {
                Style = recipe.Style;
                IsThickCrust = recipe.IsThickCrust;
                FlourType = recipe.FlourType;
                Flour = recipe.Flour;
                Water = recipe.Water;
                Yeast = recipe.Yeast;
                Salt = recipe.Salt;
                Toppings = recipe.Toppings;
                BakingTemp = recipe.BakingTemp;
                Rating = recipe.Rating;
                Notes = recipe.Notes;
            }
        }

        public Task<string> SerializeAsync() => Task.Run(() => JsonSerializer.Serialize(this));
    }

    public enum PizzaStyle
    {
        Neapolitan,
        NewYork,
        Roman
    }

    public enum FlourType
    {
        ZeroZero,
        AllPurpose,
        WholeWheat,
        Bread
    }

    public enum Topping
    {
        Mushrooms,
        CherryTomatoes,
        Prosciutto,
        Chicken,
        Bacon,
        Olives
    }
}
