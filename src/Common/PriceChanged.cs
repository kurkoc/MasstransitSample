<<<<<<<< HEAD:src/Common/PriceChanged.cs
﻿namespace Common
========
﻿namespace MasstransitSample.API.Contracts
>>>>>>>> addbeb251ceefce713a739ad4a0f1a23693d81c2:src/MasstransitSample.API/Contracts/PriceChanged.cs
{
    public class PriceChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }
    }
}
