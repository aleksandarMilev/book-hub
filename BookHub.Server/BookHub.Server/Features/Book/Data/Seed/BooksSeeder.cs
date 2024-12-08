namespace BookHub.Server.Features.Book.Data.Seed
{
    using Models;

    public static class BooksSeeder
    {
        public static Book[] Seed()
            => new Book[]
                {
                    new()
                    {
                        Id = 1,
                        Title = "Pet Sematary",
                        ShortDescription = "Sometimes dead is better.",
                        LongDescription =
                            "Pet Sematary is a 1983 horror novel by American writer Stephen King. " +
                            "The story revolves around the Creed family who move into a rural home near a pet cemetery that has " +
                            "the power to resurrect the dead. However, the resurrected creatures return with sinister changes. " +
                            "The novel explores themes of grief, mortality, and the consequences of tampering with nature. " +
                            "It was nominated for a World Fantasy Award for Best Novel in 1984. " +
                            "The book was adapted into two films: one in 1989 directed by Mary Lambert and another in 2019 directed by Kevin Kölsch and " +
                            "Dennis Widmyer. In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition, " +
                            "further solidifying its status as a classic in horror literature.",
                        AverageRating = 4.25,
                        RatingsCount = 4,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
                        AuthorId = 1,
                        PublishedDate = new DateTime(1983, 11, 4),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 2,
                        Title = "Harry Potter and the Deathly Hallows",
                        ShortDescription = "The last book from the Harry Potter series.",
                        LongDescription =
                            "Harry Potter and the Deathly Hallows is a fantasy novel written by British author J.K. Rowling. " +
                            "It is the seventh and final book in the Harry Potter series and concludes the epic tale of Harry's battle against Lord Voldemort. " +
                            "The story begins with Harry, Hermione, and Ron embarking on a dangerous quest to locate and destroy Voldemort's Horcruxes, " +
                            "which are key to his immortality. Along the way, they uncover secrets about the Deathly Hallows—three powerful magical objects " +
                            "that could aid them in their fight. The book builds to an intense and emotional climax at the Battle of Hogwarts, " +
                            "where Harry confronts Voldemort for the last time. Released on 21 July 2007, the book became a cultural phenomenon, " +
                            "breaking sales records and receiving critical acclaim for its complex characters, intricate plotting, and resonant themes " +
                            "of sacrifice, friendship, and love.",
                        AverageRating = 4.75,
                        RatingsCount = 4,
                        ImageUrl = "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206",
                        AuthorId = 2,
                        PublishedDate = new DateTime(2007, 07, 21),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 3,
                        Title = "Lord of the Rings",
                        ShortDescription = "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.",
                        LongDescription =
                            "The Lord of the Rings is a high-fantasy novel written by J.R.R. Tolkien and set in the fictional world of Middle-earth. " +
                            "The story follows the journey of Frodo Baggins, a humble hobbit who inherits the One Ring—a powerful artifact created by the Dark Lord Sauron " +
                            "to control Middle-earth. Along with a fellowship of companions, Frodo sets out on a perilous mission to destroy the ring in the fires of Mount Doom, " +
                            "the only place where it can be unmade. The narrative interweaves themes of friendship, courage, sacrifice, and the corrupting influence of power. " +
                            "Written in stages between 1937 and 1949, the novel is widely regarded as one of the greatest works of fantasy literature, " +
                            "influencing countless authors and spawning adaptations, including Peter Jackson's acclaimed film trilogy. " +
                            "With over 150 million copies sold, it remains one of the best-selling books of all time, praised for its richly detailed world-building, " +
                            "complex characters, and timeless appeal.",
                        AverageRating = 4.67,
                        RatingsCount = 6,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif",
                        AuthorId = 3,
                        PublishedDate = new DateTime(1954, 07, 29),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 4,
                        Title = "1984",
                        ShortDescription = "A dystopian novel exploring the dangers of totalitarianism.",
                        LongDescription =
                            "1984, written by George Orwell, is a dystopian novel set in a totalitarian society under the omnipresent surveillance " +
                            "of Big Brother. Published in 1949, the story follows Winston Smith, a low-ranking member of the Party, as he secretly rebels against " +
                            "the oppressive regime. Through his illicit love affair with Julia and his pursuit of forbidden knowledge, Winston challenges the Party's " +
                            "control over truth, history, and individuality. The novel introduces concepts such as 'doublethink,' 'Newspeak,' and 'thoughtcrime,' " +
                            "which have since become part of modern political discourse. Widely regarded as a classic of English literature, 1984 is a chilling exploration " +
                            "of propaganda, censorship, and the erosion of personal freedoms, serving as a cautionary tale for future generations.",
                        AverageRating = 4.8,
                        RatingsCount = 5,
                        ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1327144697i/3744438.jpg",
                        AuthorId = 5,
                        PublishedDate = new DateTime(1949, 6, 8),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 5,
                        Title = "One Flew Over the Cuckoo's Nest",
                        ShortDescription = "A story about individuality and institutional control.",
                        LongDescription =
                            "One Flew Over the Cuckoo's Nest, a novel by Ken Kesey, takes place in a mental institution " +
                            "and explores themes of individuality, freedom, and rebellion against oppressive systems. The protagonist, Randle P. McMurphy, " +
                            "a charismatic convict, fakes insanity to serve his sentence in a psychiatric hospital instead of prison. " +
                            "He clashes with Nurse Ratched, the authoritarian head nurse, and inspires the other patients to assert their independence. " +
                            "The story, narrated by Chief Bromden, a silent observer and fellow patient, examines the dynamics of power and the human spirit's resilience. " +
                            "Published in 1962, the book was adapted into a 1975 film that won five Academy Awards. It remains a poignant critique of institutional " +
                            "control and a celebration of nonconformity.",
                        AverageRating = 4.83,
                        RatingsCount = 6,
                        ImageUrl = "https://m.media-amazon.com/images/I/61Lpsc7B3jL.jpg",
                        AuthorId = 4,
                        PublishedDate = new DateTime(1962, 2, 1),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 6,
                        Title = "The Shining",
                        ShortDescription = "A chilling tale of isolation and madness.",
                        LongDescription =
                            "The Shining, written by Stephen King, is a psychological horror novel set in the remote Overlook Hotel. " +
                            "Jack Torrance, an aspiring writer and recovering alcoholic, takes a job as the hotel's winter caretaker, bringing his wife Wendy " +
                            "and young son Danny with him. Danny possesses 'the shining,' a psychic ability that allows him to see the hotel's horrific past. " +
                            "As winter sets in, the isolation and supernatural forces within the hotel drive Jack into a murderous frenzy, threatening his family. " +
                            "Published in 1977, The Shining explores themes of addiction, domestic violence, and the fragility of sanity. The novel was adapted into " +
                            "a 1980 film by Stanley Kubrick, though it diverged from King's vision. A sequel, Doctor Sleep, was published in 2013, continuing Danny's story.",
                        AverageRating = 4.83,
                        RatingsCount = 6,
                        ImageUrl = "https://m.media-amazon.com/images/I/91U7HNa2NQL._AC_UF1000,1000_QL80_.jpg",
                        AuthorId = 1,
                        PublishedDate = new DateTime(1977, 1, 28),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 7,
                        Title = "Dracula",
                        ShortDescription = "A gothic horror novel about the legendary vampire Count Dracula.",
                        LongDescription =
                            "Dracula, written by Bram Stoker, is a classic Gothic horror novel that tells the story of Count Dracula's attempt " +
                            "to move from Transylvania to England in order to spread the undead curse, and his subsequent battle with a group of " +
                            "people led by the determined Professor Abraham Van Helsing. The novel is written in epistolary form, with the story " +
                            "told through letters, diary entries, newspaper clippings, and a ship's log. At the heart of Dracula is the struggle " +
                            "between good and evil, as the characters fight to destroy the vampire lord who threatens to spread his dark influence. " +
                            "Themes of fear, desire, sexuality, and superstition permeate the novel, along with reflections on Victorian society's " +
                            "attitudes toward these topics. Published in 1897, Dracula has had a profound impact on the vampire genre, inspiring " +
                            "numerous adaptations in film, television, and popular culture. The novel explores the dangers of unchecked power, the " +
                            "mystery of the unknown, and the terror of the supernatural, cementing Count Dracula as one of literature's most " +
                            "famous and enduring villains.",
                        AverageRating = 4.6,
                        RatingsCount = 6,
                        ImageUrl = "https://m.media-amazon.com/images/I/91wOUFZCE+L._UF1000,1000_QL80_.jpg",
                        AuthorId = 6,
                        PublishedDate = new DateTime(1897, 5, 26),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 8,
                        Title = "It",
                        ShortDescription = "A terrifying tale of childhood fears, and the evil that lurks beneath the surface of a small town.",
                        LongDescription =
                            "Published in 1986, It is one of Stephen King's most iconic novels, a gripping horror story about the terror " +
                            "that haunts the small town of Derry, Maine. The novel follows a group of childhood friends who come together as adults to confront " +
                            "an ancient evil that takes the form of Pennywise, a shape-shifting entity that primarily appears as a killer clown. The novel explores " +
                            "the power of fear, the strength of friendship, and the eternal battle between good and evil. As the friends, who call themselves 'The " +
                            "Losers,' face off against Pennywise, they must confront their own childhood traumas and deepest fears. It is a chilling exploration " +
                            "of memory, courage, and the horrors of both childhood and adulthood. With its mix of supernatural terror and profound human emotion, " +
                            "It has become a cultural touchstone, inspiring a miniseries, films, and countless discussions about its themes of fear, friendship, and " +
                            "the horrors that lie beneath the surface of everyday life. King’s masterful storytelling and vivid portrayal of childhood and fear make It " +
                            "one of the most enduring works of horror fiction in the genre.",
                        AverageRating = 5,
                        RatingsCount = 4,
                        ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1334416842i/830502.jpg",
                        AuthorId = 1,
                        PublishedDate = new DateTime(1986, 9, 15),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 9,
                        Title = "Animal Farm",
                        ShortDescription = "A satirical allegory of totalitarianism, exploring the rise of power and corruption.",
                        LongDescription =
                            "First published in 1945, Animal Farm by George Orwell is a powerful political allegory that critiques totalitarian regimes " +
                            "and explores the dangers of power and corruption. The novella is set on a farm where the animals overthrow their human " +
                            "oppressors and establish a new government, only to find that the new leadership, led by the pigs, becomes just as corrupt " +
                            "as the humans they replaced. The story, told through the animals' perspective, mirrors the events leading up to the Russian " +
                            "Revolution and the rise of Stalinism. Orwell's use of anthropomorphic animals to represent different political figures " +
                            "and ideologies makes Animal Farm both accessible and deeply poignant. Its themes of betrayal, exploitation, and the " +
                            "corruption of ideals are timeless, making Animal Farm a critical commentary on power, leadership, and the manipulation " +
                            "of the masses. The novella has had a significant impact on literature and political thought, remaining a key work in the " +
                            "discussion of totalitarianism and social justice.",
                        AverageRating = 5,
                        RatingsCount = 2,
                        ImageUrl = "https://www.bookstation.ie/wp-content/uploads/2019/05/9780141036137.jpg",
                        AuthorId = 5,
                        PublishedDate = new DateTime(1945, 8, 17),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 10,
                        Title = "A Clockwork Orange",
                        ShortDescription = "A dystopian novel that explores free will, violence, and the consequences of societal control.",
                        LongDescription =
                            "First published in 1962, A Clockwork Orange by Anthony Burgess is a controversial and thought-provoking dystopian novel " +
                            "that examines the nature of free will, violence, and the conflict between individuality and societal control. The story is set " +
                            "in a near-future society and follows Alex, a teenage delinquent who leads a gang of criminals. Alex's journey is told in a unique " +
                            "style, using a fictional slang called 'Nadsat.' The novel explores the psychological and moral implications of the state-sponsored " +
                            "efforts to 'reform' Alex, subjecting him to a form of aversion therapy that strips him of his ability to choose between good and evil. " +
                            "Through this exploration, Burgess raises important questions about free will, the ethics of punishment, and the role of the state in " +
                            "shaping individual behavior. A Clockwork Orange is both a disturbing and insightful critique of modern society and its institutions. " +
                            "The novel has become a cultural touchstone, adapted into a film by Stanley Kubrick and continuing to inspire discussions on the nature " +
                            "of freedom, control, and human nature.",
                        AverageRating = 4.3,
                        RatingsCount = 6,
                        ImageUrl = "https://m.media-amazon.com/images/I/61rZCYUYXuL._AC_UF1000,1000_QL80_.jpg",
                        AuthorId = 7,
                        PublishedDate = new DateTime(1962, 1, 1),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 11,
                        Title = "Murder on the Orient Express",
                        ShortDescription = "A classic Hercule Poirot mystery aboard the luxurious Orient Express, filled with twists and a brilliant solution to a baffling crime.",
                        LongDescription =
                            "Murder on the Orient Express, first published in 1934, is one of Agatha Christie’s most famous works, featuring her legendary Belgian detective " +
                            "Hercule Poirot. The story takes place aboard the luxurious train, the Orient Express, where a wealthy American passenger, Samuel Ratchett, " +
                            "is found murdered in his compartment. Poirot, who happens to be traveling on the train, is asked to investigate the crime. " +
                            "As he delves deeper into the case, Poirot uncovers a complex web of lies and hidden motives. The passengers, all seemingly innocent, " +
                            "each have something to hide, and the detective must use his sharp mind to piece together the truth. Christie’s masterful plot, full " +
                            "of twists and red herrings, keeps readers guessing until the very end. The novel explores themes of justice, revenge, and the moral " +
                            "ambiguity of crime, making it a timeless and captivating mystery that has been adapted into numerous films and television series.",
                        AverageRating = 4.25,
                        RatingsCount = 4,
                        ImageUrl = "https://lyceumtheatre.org/wp-content/uploads/2019/09/Murder-on-the-Orient-Express-WebPstr.jpg",
                        AuthorId = 8,
                        PublishedDate = new DateTime(1934, 01, 01),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 12,
                        Title = "The Murder of Roger Ackroyd",
                        ShortDescription = "A groundbreaking Hercule Poirot mystery that reshaped the detective genre with its iconic twist ending.",
                        LongDescription =
                            "The Murder of Roger Ackroyd, first published in 1926, is one of Agatha Christie's most groundbreaking works, featuring her famous detective " +
                            "Hercule Poirot. The story is set in the quiet village of King's Abbot, where the wealthy Roger Ackroyd is found murdered in his study. " +
                            "The case takes on new complexity when Poirot, who is retired in the village, is drawn into the investigation by Ackroyd's fiancée, " +
                            "Mrs. Ferrars, who has died under mysterious circumstances just days before. As Poirot begins to unravel the case, he discovers that nearly " +
                            "everyone in the village is hiding something, and he must use his unparalleled skills of deduction to piece together the truth. " +
                            "Christie’s brilliant twist ending revolutionized the genre and is still one of the most celebrated and discussed endings in detective fiction. " +
                            "The novel touches on themes of deceit, betrayal, and the nature of truth, making it a timeless classic in the mystery genre.",
                        AverageRating = 4.7,
                        RatingsCount = 3,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/en/5/57/The_Murder_of_Roger_Ackroyd_First_Edition_Cover_1926.jpg",
                        AuthorId = 8,
                        PublishedDate = new DateTime(1926, 01, 01),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 13,
                        Title = "East of Eden",
                        ShortDescription = "A sprawling, multigenerational story of good and evil, focusing on two families in California's Salinas Valley.",
                        LongDescription =
                            "East of Eden, published in 1952, is one of John Steinbeck’s most ambitious and famous works. The novel explores the complex relationships " +
                            "between two families, the Trasks and the Hamiltons, in California's Salinas Valley during the early 20th century. Central to the narrative are " +
                            "the themes of good versus evil, inherited sin, and the choices that define our lives. Steinbeck uses the biblical story of Cain and Abel as a " +
                            "backdrop, drawing parallels between the characters' struggles and moral dilemmas. The novel is a sweeping exploration of human nature, as well " +
                            "as the destructive impact of jealousy, desire, and guilt. With its vivid descriptions of the California landscape and rich characterizations, " +
                            "East of Eden is considered one of Steinbeck's greatest works, illustrating his deep understanding of the human condition.",
                        AverageRating = 4.5,
                        RatingsCount = 4,
                        ImageUrl = "https://m.media-amazon.com/images/I/91gmBp2wQNL._AC_UF894,1000_QL80_.jpg",
                        AuthorId = 9,
                        PublishedDate = new DateTime(1952, 01, 01),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 14,
                        Title = "Dune",
                        ShortDescription = "A sweeping science fiction epic set on the desert planet Arrakis, exploring politics, religion, and ecology.",
                        LongDescription =
                            "Dune, first published in 1965, is Frank Herbert's most iconic and influential novel, often regarded as one of the greatest works of science " +
                            "fiction. The novel is set on the desert planet of Arrakis, also known as Dune, the only source of the most valuable substance in the universe, " +
                            "spice melange. The story follows Paul Atreides, the young heir to House Atreides, as he navigates political intrigue, power struggles, and the " +
                            "harsh desert environment. The novel touches on themes of power, ecology, religion, and the future of humanity. Herbert creates a detailed world " +
                            "filled with complex social, political, and ecological systems that interweave throughout the narrative. *Dune* is renowned for its intricate " +
                            "plotting, philosophical depth, and exploration of human potential. Its impact on both science fiction and modern literature is immeasurable, " +
                            "and it remains a defining work of the genre.",
                        AverageRating = 4.67,
                        RatingsCount = 3,
                        ImageUrl = "https://www.book.store.bg/dcrimg/340714/dune.jpg",
                        AuthorId = 10,
                        PublishedDate = new DateTime(1965, 01, 01),
                        IsApproved = true
                    },
                    new()
                    {
                        Id = 15,
                        Title = "The Hitchhiker's Guide to the Galaxy",
                        ShortDescription = "A comedic science fiction adventure that follows Arthur Dent's absurd journey through space after Earth is destroyed.",
                        LongDescription =
                            "The Hitchhiker's Guide to the Galaxy, first published in 1979, is Douglas Adams' most famous and influential work, blending science fiction, humor, and satire. " +
                            "The story begins with Arthur Dent, an ordinary man, who is suddenly whisked away from Earth just before it is destroyed to make way for an intergalactic freeway. Arthur " +
                            "joins Ford Prefect, an alien researcher for the titular Guide, on a wild journey through space, encountering strange planets, peculiar beings, and the galaxy's most " +
                            "incompetent bureaucracy. The novel explores themes of the absurdity of life, the meaning of existence, and the randomness of the universe, all wrapped in Adams' " +
                            "signature wit and absurdity. Known for its irreverence and humor, *The Hitchhiker's Guide to the Galaxy* has become a cult classic, inspiring numerous adaptations in " +
                            "radio, television, and film. Its influence on science fiction and comedy continues to resonate with readers and fans worldwide.",
                        AverageRating = 4.5,
                        RatingsCount = 4,
                        ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1404613595i/13.jpg",
                        AuthorId = 11,
                        PublishedDate = new DateTime(1979, 01, 01),
                        IsApproved = true
                    }
            };
    }
}
