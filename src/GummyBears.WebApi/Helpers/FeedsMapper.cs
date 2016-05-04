using GummyBears.Contracts;
using GummyBears.Entities;
using System;

namespace GummyBears.WebApi.Helpers
{
    public static class FeedsMapper
    {
        public static FeedsEntity ToEntity(this Feed feed)
        {
            return new FeedsEntity
            {
                Text = feed.Text,
                AuthorId = feed.AuthorId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}