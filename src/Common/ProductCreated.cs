
﻿namespace Common;

    public class ProductCreated
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; } = Random.Shared.Next(1,10);
    }

