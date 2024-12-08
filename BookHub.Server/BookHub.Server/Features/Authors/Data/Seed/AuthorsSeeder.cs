namespace BookHub.Server.Features.Authors.Data.Seed
{
    using Models;

    public static class AuthorsSeeder
    {
        public static Author[] Seed()
            => new Author[]
            {
                new()
                {
                    Id = 1,
                    Name = "Stephen King",
                    ImageUrl = "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg",
                    Biography =
                        "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, " +
                        "and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as " +
                        "one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which " +
                        "have been adapted into feature films, miniseries, television series, and comic books. Notable works include Carrie, The Shining, " +
                        "IT, Misery, Pet Sematary, and The Dark Tower series. King is renowned for his ability to create compelling, complex characters " +
                        "and for his mastery of building suspenseful, intricately woven narratives. Aside from his novels, he has written nearly " +
                        "200 short stories, most of which have been compiled into collections such as Night Shift, Skeleton Crew, and " +
                        "Everything's Eventual. He has received numerous awards for his contributions to literature, including the " +
                        "National Book Foundation's Medal for Distinguished Contribution to American Letters in 2003. King often writes under " +
                        "the pen name 'Richard Bachman,' a pseudonym he used to publish early works such as Rage and The Long Walk. He lives in " +
                        "Bangor, Maine, with his wife, fellow novelist Tabitha King, and continues to write and inspire new generations of " +
                        "readers and writers.",
                    PenName = "Richard Bachman",
                    AverageRating = 4.7,
                    NationalityId = 182,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1947, 09, 21),
                    IsApproved = true
                },

                new()
                {
                    Id = 2,
                    Name = "Joanne Rowling",
                    ImageUrl = "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg",
                    Biography =
                        "Joanne Rowling (born July 31, 1965), known by her pen name J.K. Rowling, is a British author and philanthropist. " +
                        "She is best known for writing the Harry Potter series, a seven-book fantasy saga that has become a global phenomenon. " +
                        "The series has sold over 600 million copies, been translated into 84 languages, and inspired a massive multimedia franchise, " +
                        "including blockbuster films, stage plays, video games, and theme parks. Notable works include Harry Potter and the " +
                        "Philosopher's Stone, Harry Potter and the Deathly Hallows, and The Tales of Beedle the Bard. Rowling's writing is praised for its " +
                        "imaginative world-building, compelling characters, and exploration of themes such as love, loyalty, and the battle between good " +
                        "and evil. After completing the Harry Potter series, Rowling transitioned to writing for adults, debuting with The Casual Vacancy, " +
                        "a contemporary social satire. She also writes crime fiction under the pseudonym Robert Galbraith, authoring the acclaimed " +
                        "Cormoran Strike series. Rowling has received numerous awards and honors for her literary achievements, including the " +
                        "Order of the British Empire (OBE) for services to children’s literature. She is an advocate for various charitable causes " +
                        "and founded the Volant Charitable Trust to combat social inequality. Rowling lives in Scotland with her family and continues " +
                        "to write, inspiring readers of all ages with her imaginative storytelling and philanthropy.",
                    PenName = "J. K. Rowling",
                    AverageRating = 4.75,
                    NationalityId = 181,
                    Gender = Gender.Female,
                    BornAt = new DateTime(1965, 07, 31),
                    IsApproved = true
                },
                new()
                {
                    Id = 3,
                    Name = "John Ronald Reuel Tolkien ",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg",
                    Biography =
                        "John Ronald Reuel Tolkien (January 3, 1892 – September 2, 1973) was an English writer, philologist, and academic. He is best " +
                        "known as the author of The Hobbit and The Lord of the Rings, two of the most beloved works in modern fantasy literature. " +
                        "Tolkien's work has had a profound impact on the fantasy genre, establishing many of the conventions and archetypes that define it " +
                        "today. Set in the richly detailed world of Middle-earth, his stories feature intricate mythologies, languages, and histories, " +
                        "reflecting his scholarly expertise in philology and medieval studies. The Hobbit (1937) was a critical and commercial success, " +
                        "leading to the epic sequel The Lord of the Rings trilogy (1954–1955), which has sold over 150 million copies and been adapted " +
                        "into award-winning films by director Peter Jackson. Tolkien also authored The Silmarillion, a collection of myths and legends " +
                        "that expand the lore of Middle-earth. He served as a professor of Anglo-Saxon at the University of Oxford, where he was a member " +
                        "of the literary group The Inklings, alongside C.S. Lewis. Tolkien's contributions to literature earned him global acclaim and a " +
                        "lasting legacy as the 'father of modern fantasy.' He passed away in 1973, but his works continue to captivate readers worldwide.",
                    PenName = "J.R.R Tolkien",
                    AverageRating = 4.67,
                    NationalityId = 181,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1892, 01, 03),
                    DiedAt = new DateTime(1973, 09, 02),
                    IsApproved = true
                },
                new()
                {
                    Id = 4,
                    Name = "Ken Kesey",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/9/9b/Ken_Kesey%2C_American_author%2C_1935-2001.jpg",
                    Biography =
                        "Ken Elton Kesey (September 17, 1935 – November 10, 2001) was an American novelist, essayist, and countercultural figure. He is " +
                        "best known for his debut novel One Flew Over the Cuckoo's Nest (1962), which explores themes of individuality, authority, and " +
                        "mental health. The novel became an instant classic, earning critical acclaim for its portrayal of life inside a psychiatric " +
                        "institution and inspiring a celebrated 1975 film adaptation starring Jack Nicholson that won five Academy Awards. " +
                        "Kesey was deeply involved in the counterculture movement of the 1960s, becoming a key figure among the Merry Pranksters, " +
                        "a group famous for their cross-country bus trip chronicled in Tom Wolfe's The Electric Kool-Aid Acid Test. " +
                        "His second novel, Sometimes a Great Notion (1964), was praised for its ambitious narrative structure and portrayal of family " +
                        "dynamics. Kesey's work often reflects his fascination with the human condition, rebellion, and the nature of freedom. " +
                        "In addition to his literary career, Kesey experimented with psychedelics, drawing inspiration from his participation in government " +
                        "experiments and his own personal experiences. He remained an influential voice in American literature and culture, inspiring " +
                        "generations of readers and writers to question societal norms and explore new perspectives. Kesey passed away in 2001, leaving " +
                        "behind a legacy of bold storytelling and a spirit of rebellion.",
                    PenName = null,
                    AverageRating = 4.6,
                    NationalityId = 182,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1935, 09, 17),
                    DiedAt = new DateTime(2001, 11, 10),
                    IsApproved = true
                },
                new()
                {
                    Id = 5,
                    Name = "George Orwell",
                    ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/george-orwell.jpg",
                    Biography =
                        "Eric Arthur Blair (June 25, 1903 – January 21, 1950), known by his pen name George Orwell, was an English novelist, essayist, " +
                        "journalist, and critic. Orwell is best known for his works Animal Farm (1945) and Nineteen Eighty-Four (1949), both of which " +
                        "are considered cornerstones of modern English literature and have had a lasting impact on political thought. Animal Farm, " +
                        "an allegorical novella satirizing the Russian Revolution and the rise of Stalinism, has become a standard in literature about " +
                        "totalitarianism and political corruption. Nineteen Eighty-Four, a dystopian novel set in a totalitarian society controlled by " +
                        "Big Brother, has influenced political discourse around surveillance, propaganda, and government control. Orwell's writings often s" +
                        "explore themes of social injustice, totalitarianism, and the misuse of power. He was an ardent critic of fascism and communism " +
                        "and was deeply involved in political activism, including fighting in the Spanish Civil War, which influenced his strong " +
                        "anti-authoritarian views. Orwell's style is known for its clarity, precision, and biting social commentary, and his work continues " +
                        "to be relevant and influential in discussions on politics, language, and individual rights. Orwell passed away in 1950 at the age " +
                        "of 46, but his work remains widely read and influential, especially in the context of discussions surrounding state power, civil " +
                        "liberties, and the role of the individual in society.",
                    PenName = "George Orwell",
                    AverageRating = 4.9,
                    NationalityId = 181,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1903, 06, 25),
                    DiedAt = new DateTime(1950, 01, 21),
                    IsApproved = true
                },
                new()
                {
                    Id = 6,
                    Name = "Bram Stoker",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                    Biography =
                        "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula, " +
                        "which has become a classic of literature and a foundational work in the vampire genre. Born in Dublin, Ireland, Stoker was " +
                        "a writer, theater manager, and journalist. While Dracula is his most famous work, Stoker wrote numerous other novels, " +
                        "short stories, and essays. He was greatly influenced by Gothic fiction and folklore, particularly Eastern European myths " +
                        "about vampires, which he drew upon to create his iconic antagonist, Count Dracula. Stoker's Dracula introduced the " +
                        "figure of the vampire into mainstream literature and has inspired countless adaptations in film, theater, and popular culture. " +
                        "Though Stoker's works initially received mixed critical reception, Dracula gained increasing recognition and has had a " +
                        "lasting impact on the horror genre. Stoker's writing was often characterized by vivid, atmospheric descriptions and " +
                        "psychological complexity, and his portrayal of fear, desire, and the supernatural remains influential today. Stoker passed " +
                        "away in 1912 at the age of 64, but his legacy as the creator of one of literature's most enduring villains continues to " +
                        "captivate readers around the world.",
                    PenName = "Bram Stoker",
                    AverageRating = 4.6,
                    NationalityId = 78,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1847, 11, 08),
                    DiedAt = new DateTime(1912, 04, 20),
                    IsApproved = true
                },
                new()
                {
                    Id = 7,
                    Name = "Anthony Burgess",
                    ImageUrl = "https://grahamholderness.com/wp-content/uploads/2019/04/anthony-burgess-at-home-i-009.jpg",
                    Biography =
                        "John Anthony Burgess Wilson (February 25, 1917 – November 25, 1993) was an English author, composer, and linguist, best known for " +
                        "his dystopian novel A Clockwork Orange. Born in Manchester, England, Burgess worked in a variety of fields before becoming a " +
                        "full-time writer. His work often explores themes of free will, violence, and social control. A Clockwork Orange, published in 1962, " +
                        "became a landmark work in the genre of dystopian fiction, and the controversial novel was later adapted into a famous film by " +
                        "Stanley Kubrick. Burgess was also known for his deep knowledge of language and for his ability to create complex, linguistically " +
                        "rich narratives. Throughout his career, Burgess wrote numerous novels, short stories, plays, and essays, in addition to works of " +
                        "music and translations. His works, while not always critically lauded, gained a loyal following, and he is considered an " +
                        "influential figure in British literature. Burgess continued to write prolifically until his death in 1993 at the age of 76.",
                    PenName = "Anthony Burgess",
                    AverageRating = 4.3,
                    NationalityId = 181,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1917, 02, 25),
                    DiedAt = new DateTime(1993, 11, 25),
                    IsApproved = true
                },
                new()
                {
                    Id = 8,
                    Name = "Agatha Christie",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/cf/Agatha_Christie.png",
                    Biography =
                        "Agatha Mary Clarissa Christie (September 15, 1890 – January 12, 1976) was an English writer, widely recognized for her detective fiction. " +
                        "She is best known for creating iconic characters such as Hercule Poirot and Miss Marple, who became central figures in numerous detective novels, short stories, and plays. " +
                        "Christie’s writing is characterized by intricate plots, psychological depth, and unexpected twists. Her first novel, The Mysterious Affair at Styles, introduced the world to Hercule Poirot in 1920. " +
                        "Over the following decades, Christie would become the best-selling novelist of all time, with over two billion copies of her books sold worldwide. " +
                        "Her works have been translated into over 100 languages, and her influence on the mystery genre is profound. Christie was appointed a Dame Commander of the Order of the British Empire in 1971. " +
                        "She continued writing until her death in 1976, leaving behind a legacy that includes Murder on the Orient Express, Death on the Nile, and And Then There Were None.",
                    PenName = "Agatha Christie",
                    AverageRating = 4.48,
                    NationalityId = 181,
                    Gender = Gender.Female,
                    BornAt = new DateTime(1890, 09, 15),
                    DiedAt = new DateTime(1976, 01, 12),
                    IsApproved = true
                },
                new()
                {
                    Id = 9,
                    Name = "John Steinbeck",
                    ImageUrl = "https://m.media-amazon.com/images/M/MV5BYzBiNmIxOWYtNTc3NS00OTg4LTgxOGYtOTJmYTk3NzJhYzE1XkEyXkFqcGc@._V1_.jpg",
                    Biography =
                        "John Ernst Steinbeck Jr. (February 27, 1902 – December 20, 1968) was an American author known for his impactful portrayal of the struggles of the " +
                        "working class and the disenfranchised. His novels, such as 'The Grapes of Wrath', 'Of Mice and Men', and 'East of Eden', often explored themes of " +
                        "poverty, social injustice, and the human condition. Steinbeck won the Nobel Prize for Literature in 1962 for his realistic and imaginative writings, " +
                        "which had a lasting effect on American literature. Born in Salinas, California, Steinbeck's early works depicted life during the Great Depression, " +
                        "while his later novels explored more personal and philosophical themes. He is best remembered for his compassion for ordinary people and his " +
                        "insightful critiques of social systems. His writing style was marked by his use of vivid, poetic descriptions and a deep understanding of his " +
                        "characters' struggles.",
                    PenName = "John Steinbeck",
                    AverageRating = 4.5,
                    NationalityId = 182,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1902, 02, 27),
                    DiedAt = new DateTime(1968, 12, 20),
                    IsApproved = true
                },
                new()
                {
                    Id = 10,
                    Name = "Frank Herbert",
                    ImageUrl = "https://www.historylink.org/Content/Media/Photos/Large/Frank-Herbert-signing-books-Seattle-December-5-1971.jpg",
                    Biography =
                        "Frank Herbert (October 8, 1920 – February 11, 1986) was an American science fiction author best known for his landmark series Dune. " +
                        "Herbert's work frequently explored themes of politics, religion, and ecology. *Dune*, first published in 1965, remains one of the most influential and " +
                        "best-selling science fiction novels of all time, winning the Hugo and Nebula Awards. Born in Tacoma, Washington, Herbert began his writing career as a journalist, " +
                        "before transitioning to fiction writing. His Dune series, which consists of six novels, delves into the complexities of power, governance, environmentalism, and the human condition. " +
                        "Herbert's distinctive writing style is known for its philosophical depth, intricate world-building, and the exploration of human evolution. His work has had a profound influence on both science fiction and popular culture. " +
                        "Herbert passed away in 1986, but his legacy continues through his novels, which are still widely read and respected today.",
                    PenName = "Frank Herbert",
                    AverageRating = 4.67,
                    NationalityId = 182,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1920, 10, 08),
                    DiedAt = new DateTime(1986, 02, 11),
                    IsApproved = true
                },
                new()
                {
                    Id = 11,
                    Name = "Douglas Adams",
                    ImageUrl = "https://static.tvtropes.org/pmwiki/pub/images/DouglasAdams_douglasadams_com.jpg",
                    Biography =
                        "Douglas Adams (March 11, 1952 – May 11, 2001) was an English author, best known for his science fiction series The Hitchhiker's Guide to the Galaxy. " +
                        "Adams’ works are renowned for their wit, absurdity, and philosophical depth. Born in Cambridge, England, Adams began his career in comedy writing before transitioning into " +
                        "fiction. The Hitchhiker's Guide to the Galaxy, first published as a radio play in 1978 and later as a novel in 1979, became a cultural phenomenon, blending humor, satire, and " +
                        "sci-fi elements into a beloved classic. The series, which spans multiple books, explores themes of existence, the absurdity of life, and the universe in a way that resonates with " +
                        "both fans of science fiction and general readers. Adams’ distinctive voice and irreverent humor continue to inspire and entertain readers today. He also worked as a scriptwriter " +
                        "and wrote other notable works, including Dirk Gently's Holistic Detective Agency. Adams passed away in 2001 at the age of 49, but his influence on the science fiction genre and " +
                        "popular culture remains enduring.",
                    PenName = "Douglas Adams",
                    AverageRating = 4.5,
                    NationalityId = 181,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1952, 03, 11),
                    DiedAt = new DateTime(2001, 05, 11),
                    IsApproved = true
                }
            };
    }
}
