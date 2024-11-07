<<<<<<<< HEAD:src/Common/ProductCreated.cs
﻿namespace Common
========
﻿namespace MasstransitSample.API.Contracts
>>>>>>>> addbeb251ceefce713a739ad4a0f1a23693d81c2:src/MasstransitSample.API/Contracts/ProductCreated.cs
{
    public class ProductCreated
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; } = 3;
    }
}
