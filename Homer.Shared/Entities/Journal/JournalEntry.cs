using System;

namespace Homer.Shared.Entities.Journal
{
    public class JournalEntry
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public string Mood { get; set; }

        public string Personal { get; set; }
        public string Work { get; set; }
    }
}