namespace BookHub.Server.Data.Seed
{
    using Models;

    public static class UsersSeeder
    {
        public static User[] Seed()
            => new User[]
            {
                new(){ Id = "user1Id", UserName = "user1name", Email = "user1@mail.com" },
                new(){ Id = "user2Id", UserName = "user2name", Email = "user2@mail.com" },
                new(){ Id = "user3Id", UserName = "user3name", Email = "user3@mail.com" },
            };
    }
}
