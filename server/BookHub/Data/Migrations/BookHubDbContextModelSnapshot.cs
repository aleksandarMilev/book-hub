#nullable disable
namespace BookHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    [DbContext(typeof(BookHubDbContext))]
    partial class BookHubDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookHub.Server.Data.Models.Shared.BookGenre.BookGenre", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("BooksGenres");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            GenreId = 1
                        },
                        new
                        {
                            BookId = 1,
                            GenreId = 4
                        },
                        new
                        {
                            BookId = 1,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 1,
                            GenreId = 30
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 7
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 14
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 15
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 32
                        },
                        new
                        {
                            BookId = 3,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 3,
                            GenreId = 7
                        },
                        new
                        {
                            BookId = 3,
                            GenreId = 24
                        },
                        new
                        {
                            BookId = 4,
                            GenreId = 19
                        },
                        new
                        {
                            BookId = 4,
                            GenreId = 25
                        },
                        new
                        {
                            BookId = 4,
                            GenreId = 28
                        },
                        new
                        {
                            BookId = 4,
                            GenreId = 29
                        },
                        new
                        {
                            BookId = 5,
                            GenreId = 13
                        },
                        new
                        {
                            BookId = 5,
                            GenreId = 19
                        },
                        new
                        {
                            BookId = 5,
                            GenreId = 26
                        },
                        new
                        {
                            BookId = 5,
                            GenreId = 28
                        },
                        new
                        {
                            BookId = 5,
                            GenreId = 29
                        },
                        new
                        {
                            BookId = 6,
                            GenreId = 1
                        },
                        new
                        {
                            BookId = 6,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 6,
                            GenreId = 29
                        },
                        new
                        {
                            BookId = 6,
                            GenreId = 30
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 1
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 7
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 8
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 30
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 31
                        },
                        new
                        {
                            BookId = 8,
                            GenreId = 1
                        },
                        new
                        {
                            BookId = 8,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 8,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 8,
                            GenreId = 7
                        },
                        new
                        {
                            BookId = 9,
                            GenreId = 19
                        },
                        new
                        {
                            BookId = 9,
                            GenreId = 25
                        },
                        new
                        {
                            BookId = 9,
                            GenreId = 28
                        },
                        new
                        {
                            BookId = 9,
                            GenreId = 29
                        },
                        new
                        {
                            BookId = 10,
                            GenreId = 19
                        },
                        new
                        {
                            BookId = 10,
                            GenreId = 21
                        },
                        new
                        {
                            BookId = 10,
                            GenreId = 26
                        },
                        new
                        {
                            BookId = 10,
                            GenreId = 28
                        },
                        new
                        {
                            BookId = 10,
                            GenreId = 29
                        },
                        new
                        {
                            BookId = 11,
                            GenreId = 4
                        },
                        new
                        {
                            BookId = 11,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 11,
                            GenreId = 21
                        },
                        new
                        {
                            BookId = 12,
                            GenreId = 4
                        },
                        new
                        {
                            BookId = 12,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 12,
                            GenreId = 21
                        },
                        new
                        {
                            BookId = 13,
                            GenreId = 8
                        },
                        new
                        {
                            BookId = 13,
                            GenreId = 13
                        },
                        new
                        {
                            BookId = 13,
                            GenreId = 26
                        },
                        new
                        {
                            BookId = 13,
                            GenreId = 28
                        },
                        new
                        {
                            BookId = 14,
                            GenreId = 2
                        },
                        new
                        {
                            BookId = 14,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 14,
                            GenreId = 7
                        },
                        new
                        {
                            BookId = 14,
                            GenreId = 24
                        },
                        new
                        {
                            BookId = 14,
                            GenreId = 25
                        },
                        new
                        {
                            BookId = 15,
                            GenreId = 2
                        },
                        new
                        {
                            BookId = 15,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 15,
                            GenreId = 16
                        });
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Shared.ChatUser.ChatUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<bool>("HasAccepted")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "ChatId");

                    b.HasIndex("ChatId");

                    b.ToTable("ChatsUsers");
                });

            modelBuilder.Entity("BookHub.Server.Features.Article.Data.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting for a nightmare that challenges Louis' understanding of life, death, and morality. \n\nThe cemetery behind the Creed family’s house is a place that defies the natural order, capable of bringing the dead back to life—albeit in a twisted, unnatural form. The temptation to reverse the inevitable loss of loved ones soon becomes too great to ignore. Louis, who is already grappling with the recent death of his son's cat, Church, learns that there is another cemetery—an even darker place—that has the power to raise humans from the dead. The price of this power, however, is steep, and the consequences far-reaching. When Louis makes the fateful decision to bring Church back from the grave, he begins a journey that will lead him down a path of moral corruption, despair, and irreversible tragedy. \n\nAs the story unfolds, King masterfully weaves themes of family, loss, and the struggles of parenthood. Louis' motivations are rooted in love for his family, particularly for his children, Ellie and Gage. His desire to protect them from the pain of loss becomes an overwhelming force, overshadowing his ability to consider the consequences. His wife Rachel, who harbors her own deep-seated fears about death due to the trauma of losing her sister Zelda to a debilitating illness, is caught in her own web of denial. She represents the fragility of human nature and the lengths people will go to avoid facing their darkest fears. The tension between Louis' desire to preserve life and Rachel’s unwillingness to confront death creates a tragic dynamic that becomes central to the novel’s emotional core. \n\nKing’s portrayal of death is particularly haunting in Pet Sematary. He doesn’t just focus on the physical act of dying; instead, he delves deeply into the psychological, emotional, and spiritual toll death takes on individuals and families. The novel is filled with gut-wrenching scenes that force readers to confront the reality that death is not just the end of a person’s life, but also the end of the relationships, dreams, and futures that are intertwined with them. Louis’ journey is a devastating reflection on the lengths people are willing to go to undo the pain of losing a loved one, even if it means sacrificing their humanity in the process. \n\nIn Pet Sematary, King’s signature blend of supernatural horror and psychological depth is on full display. The power of the cemetery is a metaphor for the human desire to control the uncontrollable—to bend fate to one’s will. The cemetery represents the dangerous temptation of hubris, a theme that has been explored in countless stories across literature and folklore. King takes this familiar theme and brings it to a new level of horror, showing how the attempt to reverse death leads not to salvation, but to destruction. The creatures that return from the cemetery are not the same as they were in life; they are tainted, twisted by the unnatural forces that brought them back. This raises unsettling questions about the nature of life and death—what makes us human, and what happens to us when the natural order is disrupted. \n\nOne of the most terrifying aspects of the novel is its exploration of the idea that some things are better left untouched. The moral lesson of Pet Sematary is a powerful warning about the dangers of trying to undo the irreversible. It serves as a meditation on the importance of accepting loss, moving through grief, and letting go. In trying to alter fate, Louis not only brings tragedy to his family, but also damns himself in the process. His decision is a desperate attempt to hold onto the past, but in doing so, he loses everything that matters in the present. The cost of defying death is the ultimate price: the loss of one’s soul. \n\nPet Sematary stands as one of King’s most deeply emotional works. It is a story that reminds us of the importance of embracing life’s fleeting moments and coming to terms with our own mortality. Though the novel is often categorized as a horror story, its true horror lies not in the supernatural events, but in the internal struggles of the characters—in their desperate attempts to avoid facing the unavoidable. King’s writing is a masterclass in creating tension and unease, building dread not just through external events, but through the complex emotional landscapes of his characters. The novel’s haunting final scenes linger long after the last page is turned, leaving readers to reflect on the true meaning of life, love, and loss.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8543),
                            ImageUrl = "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg",
                            Introduction = "A tale of love, loss, and the chilling cost of defying death.",
                            IsDeleted = false,
                            Title = "Exploring the Haunting Depths of Pet Sematary",
                            Views = 0
                        },
                        new
                        {
                            Id = 2,
                            Content = "Harry Potter and the Deathly Hallows is not just the final book in J. K. Rowling’s iconic series, but the culmination of a journey that began with a young, orphaned wizard discovering his destiny in a world full of magic, mystery, and danger. The seventh and final installment brings to a close the epic battle between Harry Potter and the dark wizard Lord Voldemort, and it is a story that revolves around themes of love, sacrifice, loyalty, and the ultimate quest for truth. As the story reaches its climax, readers are taken on a journey not only through the magical world but also through the very heart of what it means to face evil, to stand up for what is right, and to embrace the unknown. \n\nIn The Deathly Hallows, Harry, Ron, and Hermione are no longer students at Hogwarts but fugitives on the run, tasked with finding and destroying Voldemort’s Horcruxes—objects that contain pieces of his soul and which must be destroyed in order to defeat him once and for all. The trio embarks on a perilous mission, not only to protect the wizarding world but also to safeguard their loved ones from the growing power of the Dark Lord. Their journey takes them deep into the heart of the magical world, uncovering secrets from the past that could alter the future forever. As they travel from one dangerous encounter to the next, Harry comes face to face with painful truths about his own family, the past, and his role in the final battle. \n\nThe book delves into the personal lives of its beloved characters. Harry's struggle is no longer about simply surviving the trials set before him—he is now the symbol of hope and resistance against the forces of evil. Throughout the story, Harry is confronted with the burden of being ‘the chosen one’—the person who must face Voldemort in a final confrontation. He learns that his greatest strength lies not in his ability to perform magic, but in his unwavering loyalty to his friends and his deep understanding of the importance of love. Ron and Hermione also undergo incredible growth as they wrestle with their own personal fears, relationships, and responsibilities. The trio’s bond strengthens, proving that their friendship is the key to their success. The sacrifices they make along the way are deeply moving and serve as a testament to the enduring power of love and loyalty. \n\nIn the search for the Horcruxes, the trio uncovers a series of interconnected mysteries, including the tale of the Deathly Hallows. The Deathly Hallows are three magical objects that are said to grant their possessor mastery over death itself. The Elder Wand, the Resurrection Stone, and the Invisibility Cloak are legendary artifacts, and their story is deeply tied to the larger battle between Harry and Voldemort. As the Hallows' significance is revealed, Harry finds himself caught between two paths: the desire to pursue the Hallows and the need to destroy Voldemort’s Horcruxes. His eventual understanding of the Hallows helps him realize that true power lies not in conquering death, but in embracing the love, selflessness, and choices that define humanity. \n\nOne of the most significant themes in The Deathly Hallows is the idea of sacrifice. Throughout the novel, the characters are faced with the ultimate test of what they are willing to give up in order to achieve victory. Harry’s willingness to sacrifice himself for the greater good is a powerful moment in the series, a testament to his maturity and understanding of the true cost of war. It is in these moments of self-sacrifice that the characters display their most heroic qualities, showing that heroism is not defined by the glory of battle, but by the quiet acts of bravery in the face of overwhelming odds. \n\nThe final battle at Hogwarts is the climax of the series, and it is a moment of immense emotional and narrative weight. The Death Eaters, led by Voldemort, lay siege to the school, forcing the students, teachers, and members of the Order of the Phoenix to stand and fight. The battle is not just a fight for survival—it is a fight for the future of the wizarding world. As the forces of good prepare for the ultimate confrontation, it becomes clear that the series has always been about more than just defeating evil. It is about fighting for a world where love, loyalty, and hope are the guiding principles. As characters who were once side players rise to take their place in the fight, readers are reminded that courage comes from unexpected places, and that even the smallest act of bravery can change the course of history. \n\nHarry Potter and the Deathly Hallows also explores the theme of legacy—what we leave behind, and how our actions shape the future. The end of the book sees the closure of several long-standing mysteries and storylines, while also providing the series with an optimistic, yet bittersweet conclusion. The future is uncertain, but the characters find peace knowing that they gave everything for a better world. Through the lens of loss, Rowling shows that the end is not necessarily the end, but a new beginning. The epilogue, set 19 years after the final battle, offers readers a glimpse of the next generation and how the lessons learned from the past will shape the future. Harry, Ron, and Hermione have grown, matured, and become parents, passing on their own legacies of courage, kindness, and love to their children. \n\nUltimately, Harry Potter and the Deathly Hallows is a story of hope—hope in the face of fear, in the face of evil, and in the face of the unknown. It is a story about growing up, about making choices, and about the power of love to overcome even the darkest forces. Rowling's storytelling is masterful, blending action, emotion, and depth in a way that resonates with readers of all ages. As the final chapter in the Harry Potter series, The Deathly Hallows delivers a powerful message: no matter how dark the world may seem, love and loyalty will always light the way.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8594),
                            ImageUrl = "https://choicefineart.com/cdn/shop/products/book-7-harry-potter-and-the-deathly-hallows-311816.jpg?v=1688079541",
                            Introduction = "The battle against Voldemort reaches its thrilling and emotional finale.",
                            IsDeleted = false,
                            Title = "The Epic Conclusion: Harry Potter and the Deathly Hallows",
                            Views = 0
                        },
                        new
                        {
                            Id = 3,
                            Content = "The Lord of the Rings is an epic fantasy novel by J. R. R. Tolkien, widely regarded as one of the greatest works of literature in the 20th century. The novel follows the journey of a young hobbit, Frodo Baggins, as he is tasked with destroying a powerful and corrupt artifact known as the One Ring, which was created by the Dark Lord Sauron to rule over all of Middle-earth. As the story unfolds, Frodo and his companions embark on an arduous and perilous quest to reach the fires of Mount Doom, where the Ring must be destroyed in order to defeat Sauron and save their world. However, the journey is far from simple—trapped by the corrupting influence of the Ring, the fellowship of travelers must navigate treachery, loss, and immense personal challenges, all while facing the growing darkness of Sauron's forces.\n\nThe story of The Lord of the Rings is one of epic adventure and enduring friendship. At the heart of the novel is the bond between Frodo and his loyal companion, Samwise Gamgee, whose unwavering devotion to Frodo and the quest drives the emotional core of the narrative. Together, along with Aragorn, Legolas, Gimli, and others, Frodo faces seemingly insurmountable odds as they travel through treacherous lands, battling forces of evil that threaten to consume the world. The fellowship's journey represents much more than just the destruction of the Ring—it is a story of personal growth, sacrifice, and the power of hope in the face of overwhelming darkness. \n\nTolkien's mastery of world-building is evident throughout the novel, as he brings to life a richly detailed world called Middle-earth. From the peaceful Shire to the dark, desolate lands of Mordor, every part of Middle-earth is immensely vivid, filled with complex histories, diverse cultures, and fascinating creatures. The novel introduces readers to a pantheon of memorable characters, from the wise and enigmatic Gandalf the wizard to the tragic and conflicted Gollum, whose obsession with the Ring adds an unsettling layer of tension to the story. Each character in the novel has their own role to play, and they all contribute to the overarching theme of the importance of teamwork, loyalty, and friendship in the face of an increasingly dark and divided world. \n\nOne of the central themes of The Lord of the Rings is the idea of power and its ability to corrupt. The One Ring, a seemingly simple object, has the power to control the minds and hearts of those who seek to possess it. As Frodo and his companions travel toward Mount Doom, they encounter individuals and groups who fall under the Ring's corrupting influence, whether they are Sauron's minions or those who believe they can control the Ring for good. The tragic story of Boromir, who succumbs to the Ring's temptation, serves as a powerful reminder of the destructive nature of unchecked ambition and the dangers of trying to wield power without understanding its true cost. In contrast, Frodo's struggle to resist the Ring's temptation demonstrates the power of selflessness and humility, as he refuses to let his own desires define the outcome of the quest. \n\nAnother important theme explored in the novel is the idea of sacrifice. Throughout The Lord of the Rings, characters are called upon to give up something precious for the greater good. Aragorn, the rightful king of Gondor, must step into a leadership role even as he struggles with his own doubts and fears. Frodo, Sam, and their companions make tremendous sacrifices—physically, emotionally, and morally—in order to see the quest through to the end. In particular, the burden of carrying the Ring weighs heavily on Frodo, and it is only through the unwavering loyalty of Sam that Frodo is able to complete the journey. Sam’s sacrifice is profound, as he puts his own desires and happiness aside to support Frodo through the darkest moments of their journey. \n\nThe novel is also deeply concerned with the power of hope and perseverance. Even in the darkest moments, when all seems lost, the characters persist in their quest. The struggle against Sauron and his forces is one of unyielding resistance against evil, and the power of the small, seemingly insignificant individuals in the story—such as Frodo and Sam—reminds readers that even the most humble among us can play a vital role in shaping the future. Tolkien also illustrates the importance of community and solidarity in the face of adversity, as characters from different races and backgrounds come together to fight for a common cause. The diverse Fellowship of the Ring exemplifies the value of cooperation, tolerance, and mutual respect, showing that unity is the key to overcoming even the greatest of challenges. \n\nAs the novel reaches its conclusion, the fate of Middle-earth hangs in the balance. In a climactic showdown at the Black Gate of Mordor, Frodo and Sam's courage and determination lead to the destruction of the Ring and the fall of Sauron. But the victory comes at a great cost. Many lives are lost, and the characters must reconcile the sacrifices they’ve made with the world that they have saved. The closing chapters of The Lord of the Rings are marked by both the triumph of good over evil and the melancholy recognition that the world has forever changed. \n\nIn the end, The Lord of the Rings is a story about the enduring power of friendship, the consequences of wielding great power, and the importance of selflessness, sacrifice, and hope. Tolkien's work transcends the boundaries of fantasy literature, offering a timeless and deeply resonant narrative that continues to captivate readers of all ages. Whether it is the humble courage of Frodo, the noble sacrifice of Aragorn, or the steadfast loyalty of Sam, The Lord of the Rings reminds us that in the face of darkness, there is always a light to guide us—if we are brave enough to seek it.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8596),
                            ImageUrl = "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEibI7_-Az0QVZhhwZO_PcgrNRK7RYnS7JPiddt_LvTC8NTgTzzYcaagGBLR6KtgY1J_VyZzS6HhL7MW9x1h-rioISPanc-daPbdgnZCQQb48PNELDt9gbQlohCJuXGHgritNS_3Ff08oUhs/w1200-h630-p-k-no-nu/acetolkien.jpg",
                            Introduction = "A journey of courage, friendship, and the fight against overwhelming darkness.",
                            IsDeleted = false,
                            Title = "The Timeless Epic: The Lord of the Rings",
                            Views = 0
                        },
                        new
                        {
                            Id = 4,
                            Content = "The Shining is a psychological horror novel by Stephen King, first published in 1977. The story follows Jack Torrance, an aspiring writer and recovering alcoholic, who takes a job as the winter caretaker of the Overlook Hotel, a remote resort nestled in the mountains of Colorado. Jack moves to the hotel with his wife Wendy and his young son Danny, who possesses a psychic ability called 'the shining.' As the winter weather isolates the family from the outside world, the Overlook's dark, haunted history begins to unravel, and the hotel's malevolent supernatural forces begin to take control of Jack, pushing him toward violence and madness. What begins as a simple family retreat spirals into a nightmare of terror and survival, as Danny and Wendy fight to escape the hotel's deadly grip. \n\nAt the core of The Shining is the gradual descent of Jack Torrance into madness. A complex and deeply flawed character, Jack is haunted by his past mistakes, including his struggles with alcoholism and his abusive relationship with his family. As Jack becomes more influenced by the sinister forces within the hotel, his inner turmoil and his violent tendencies are amplified, and he slowly becomes a threat to his own family. The novel portrays the terrifying effects of isolation, desperation, and personal demons, making Jack's descent into madness both heartbreaking and horrifying. \n\nThe relationship between Jack and his family, particularly his wife Wendy and son Danny, is one of the most important elements of the novel. Wendy is a strong and resourceful character, doing everything she can to protect her son and preserve the family unit in the face of increasing danger. Danny, with his psychic ability, serves as both a victim and a savior, as his 'shining' allows him to sense the evil forces at work within the hotel. Danny's bond with Wendy is central to the emotional core of the novel, as they both struggle to survive against the malevolent force trying to consume them. Their relationship is a powerful representation of love and loyalty in the face of unimaginable horror. \n\nKing's mastery of psychological horror is evident in the novel's chilling atmosphere, which builds steadily throughout the story. The Overlook Hotel, with its vast, empty halls and eerie, oppressive presence, serves as a character in its own right, slowly driving Jack to madness and serving as a dark mirror to the family's fractured relationships. King uses the setting to evoke a constant sense of dread, with the hotel's ghosts and supernatural phenomena representing both the past sins of its former occupants and the growing malevolence that threatens the Torrance family. \n\nOne of the central themes of The Shining is the idea of the supernatural and its influence on human behavior. While the novel is filled with terrifying encounters and ghostly apparitions, the true horror lies in how the hotel’s evil influences the minds of its inhabitants. The Overlook represents the corruption of power, and its ability to manipulate Jack into violence is a reflection of how external forces can prey on internal vulnerabilities. The story also delves into the idea of destiny, as Danny’s 'shining' allows him to foresee the danger that lies ahead, even though he cannot fully comprehend the extent of the evil within the hotel. Danny’s ability to sense the past, present, and future creates a sense of fate and inevitability that permeates the novel. \n\nAnother key theme in The Shining is the effect of isolation on the human psyche. Trapped in a remote location with no contact with the outside world, the Torrance family is forced to confront their personal demons and unravel the mysteries of the Overlook. Jack’s internal struggles, combined with the hotel’s supernatural power, push him further towards violence and madness. The novel explores the ways in which the isolation of the winter season, coupled with the psychological weight of past trauma, creates an environment ripe for horror. \n\nHope and survival are also prominent themes, as Danny and Wendy's perseverance and love for each other become their only means of survival. While Jack succumbs to the hotel's power, Danny and Wendy fight with everything they have to escape the nightmare. Their struggle against the Overlook represents the resilience of the human spirit in the face of overwhelming evil. \n\nAs the novel reaches its climactic moments, the tension reaches its peak. In a final confrontation between father and son, Danny’s psychic abilities and Wendy’s determination lead to the destruction of the malevolent forces within the hotel, but not without significant cost. Jack, overwhelmed by the power of the Overlook, meets his tragic end, and the family must leave the hotel behind, scarred but alive. The conclusion of The Shining leaves readers with a sense of both horror and catharsis, as the characters escape the hotel's grip but are forever haunted by the events they experienced. \n\nIn the end, The Shining is a story about the fragility of the human mind, the destructive nature of addiction, and the dangerous consequences of isolation. Stephen King's novel delves into the darkness of the human soul, showing how the line between reality and madness can blur in the face of intense fear and pressure. It remains one of King’s most haunting and enduring works, cementing his reputation as the master of psychological horror. Whether it's the suffocating atmosphere of the Overlook or the chilling unraveling of Jack Torrance's sanity, The Shining continues to captivate and terrify readers, proving that sometimes the greatest horror lies not in what we see, but in what we cannot escape from within.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8599),
                            ImageUrl = "https://kcopera.org/wp-content/uploads/2024/03/the-shining-recording-page-banner.jpg",
                            Introduction = "A chilling exploration of isolation, madness, and the supernatural, The Shining takes readers deep into the heart of terror.",
                            IsDeleted = false,
                            Title = "The Haunting Legacy of The Shining",
                            Views = 0
                        },
                        new
                        {
                            Id = 5,
                            Content = "1984 is a dystopian novel by George Orwell, first published in 1949. The novel is set in a totalitarian society ruled by the Party, a tyrannical government led by the figurehead Big Brother. In this world, individual freedom is suppressed, and citizens are constantly monitored by surveillance, subjected to propaganda, and manipulated into submission. The protagonist, Winston Smith, works for the Party rewriting history in order to align it with the Party’s ever-changing narrative. However, Winston harbors rebellious thoughts, longing for truth and freedom. As Winston embarks on a secret affair with Julia, a fellow Party member, and secretly seeks to resist Big Brother’s rule, he becomes entangled in a dangerous game of deception and surveillance, where the very concept of truth is in constant flux. \n\nAt the heart of 1984 is the theme of oppression and the obliteration of individuality. The Party’s control over every aspect of life is absolute: from the daily routines of citizens to the language they speak (Newspeak), the Party seeks to mold every individual thought and action. Winston’s journey is one of self-awareness, as he questions the reality that has been imposed upon him. Orwell paints a bleak picture of a world where even personal thoughts are not safe from the reach of the Party. The novel explores the horrifying consequences of a society where individuality is eradicated and citizens are reduced to mere cogs in the machinery of an all-powerful state. \n\nThe Party's control is not just through physical surveillance, but also through the manipulation of truth. One of the most striking features of Orwell's novel is the concept of doublethink—the ability to hold two contradictory beliefs at the same time and accept both as true. This is central to the Party’s strategy of controlling thought and creating a reality where nothing is ever truly certain. Winston works at the Ministry of Truth, where his job is to falsify records, erasing any evidence of the Party's lies. This manipulation of history reflects the Party’s need to control not only the present but also the past, erasing any evidence that could challenge its authority. In this world, truth is not based on objective reality but on whatever the Party decrees it to be at any given moment. \n\nOne of the central themes of 1984 is the loss of personal freedom and autonomy. The Party's dominance extends to every facet of life, creating a society where even private thoughts are policed. The concept of ‘thoughtcrime,’ the act of thinking against the Party, is one of the most terrifying elements of the novel. Winston's rebellion against the Party is not just physical but intellectual—he dares to think independently, to question the society he lives in. However, the Party's control is so complete that even Winston’s secret rebellion is eventually quashed, demonstrating the terrifying power of totalitarianism. \n\nOrwell also explores the dangers of surveillance in 1984. The Party’s slogan, ‘Big Brother is watching you,’ is a constant reminder that no one is ever truly alone or free. Telescreens, microphones, and constant surveillance are woven into the fabric of daily life. This omnipresent watchfulness stifles any possibility of rebellion, as the fear of being caught leads to self-censorship. In 1984, privacy is a foreign concept, and the government’s ability to monitor every action and thought creates a society in which no one can trust anyone, not even themselves. \n\nAnother theme in 1984 is the manipulation of language. The Party’s development of Newspeak, a language designed to eliminate words that could foster subversive thoughts, demonstrates how language can shape and control reality. Through Newspeak, the Party seeks to limit the range of thought, making it impossible for people to even think critically about the regime. By controlling language, the Party not only controls communication but also dictates how people are allowed to think, further solidifying its power over every aspect of life. \n\nAt its core, 1984 is a novel about the struggle for truth and freedom in a world where both are systematically destroyed. Winston’s quest for knowledge, individuality, and truth leads him to defy the Party, but in the end, he is broken, and the Party’s grip on reality is solidified. The tragic ending of the novel highlights the total power of the regime and the utter futility of resistance. Winston’s final acceptance of Big Brother is a horrifying testament to the complete domination of the Party over the minds and souls of its citizens. \n\nThe novel’s bleak conclusion leaves readers with a powerful message about the dangers of unchecked power, totalitarianism, and the erosion of personal freedoms. Orwell's vision of the future in 1984 serves as a warning of the potential consequences of oppressive regimes and the fragility of democracy. The novel's exploration of surveillance, mind control, and the manipulation of truth remains strikingly relevant today, as issues of government surveillance, fake news, and the battle for individual freedoms continue to challenge societies worldwide. \n\nIn the end, 1984 is not just a warning about a specific future but an exploration of the enduring struggle between freedom and oppression. Orwell’s chilling narrative of Winston’s life under Big Brother’s watchful eye remains a powerful and thought-provoking work, urging readers to consider the value of truth, individuality, and liberty in the face of ever-growing authoritarianism.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8601),
                            ImageUrl = "https://s3.amazonaws.com/adg-bucket/1984-george-orwell/3423-medium.jpg",
                            Introduction = "A chilling exploration of totalitarianism, surveillance, and the loss of individuality, 1984 presents a terrifying vision of the future.",
                            IsDeleted = false,
                            Title = "The Dystopian Reality of 1984",
                            Views = 0
                        },
                        new
                        {
                            Id = 6,
                            Content = "Dracula, written by Bram Stoker and published in 1897, is the quintessential gothic horror novel, blending superstition, desire, and dread into an unforgettable narrative. The story centers around Count Dracula, a centuries-old vampire who embarks on a journey from Transylvania to England, seeking to spread his undead curse. His arrival in England sets off a deadly chain of events, as Dracula begins his relentless pursuit of Mina Harker and her friends, leading to a fierce battle between good and evil. The novel’s form—presented through diary entries, letters, newspaper clippings, and a ship’s log—creates an immersive experience that pulls the reader into the horrifying world of Dracula’s curse. \n\nAt its core, Dracula explores the boundaries between the human world and the supernatural, dissecting themes of death, immortality, and forbidden desire. Count Dracula, an ancient and malevolent force, represents the ultimate fear of death’s corruption and the terrifying power of the undead. With his eerie castle in Transylvania, Dracula lives outside the rules of nature and morality, existing as both a seductive and monstrous figure. His very existence is an affront to the natural order, and throughout the novel, he manipulates and controls those around him to serve his insatiable hunger for blood and power. \n\nDracula’s pursuit of Mina is not just a physical conquest, but a spiritual and psychological one. The vampire’s ability to manipulate minds, corrupt souls, and sow doubt in the hearts of the innocent becomes the primary means by which he attacks. Dracula is a master of fear, slowly draining the vitality and willpower of those around him, especially Mina, who represents purity and virtue. Her transformation from an innocent young woman to a creature tainted by Dracula’s curse serves as a central theme of the novel—one that raises questions about sexual desire, repression, and the dangers of unchecked lust. Dracula’s actions echo the sexual fears of the Victorian era, as he embodies the darker, more forbidden aspects of human nature. His supernatural abilities allow him to seduce and dominate, transforming his victims into mindless, bloodthirsty followers, turning the act of bloodletting into an intimate and terrifying ritual. \n\nThe battle against Dracula is not just a fight against an individual monster, but a reflection of the ongoing struggle between civilization and savagery. The vampire represents the unrestrained forces of nature, while the protagonists—Jonathan Harker, Mina, Professor Van Helsing, Lucy, and others—symbolize the civilized world, with its belief in logic, science, and morality. Throughout the novel, the characters’ reliance on reason and rationality is tested, as Dracula’s supernatural abilities defy all understanding. However, the struggle also reveals the limits of science and the need for faith, love, and bravery in the face of unimaginable evil. As the group comes together to confront Dracula, they form a united front, using their collective strengths to overcome the vampire’s growing power. \n\nThe gothic atmosphere in Dracula is perhaps one of its most compelling features. Stoker’s descriptions of the eerie Transylvanian landscape—dark, foreboding mountains, deep forests, and the imposing Castle Dracula—imbue the novel with an almost tangible sense of dread. The supernatural horror that pervades the narrative is not just confined to Dracula’s castle but extends to England itself, where the vampire’s presence warps reality and disrupts the lives of those he encounters. In London, the fog, the shadows, and the sense of pervasive danger create a setting that feels suffocating, and the characters must constantly struggle against the unseen forces that threaten to engulf them. The tension between the known world and the unknown, represented by Dracula’s powers, creates an atmosphere of claustrophobic terror. \n\nIn addition to the terrifying supernatural elements, Dracula explores the concept of forbidden knowledge and the limits of human understanding. As the protagonists discover the vampire’s nature, they must challenge their own preconceived notions about life, death, and the supernatural. This journey into the unknown mirrors the fear of losing control over one’s own fate and the terrifying realization that knowledge—when it comes to creatures like Dracula—might be too dangerous to comprehend. Van Helsing, as the novel’s intellectual leader, is both a man of science and a believer in the supernatural, and his attempts to rationalize Dracula’s power show the tension between reason and faith. \n\nMina Harker, though a central figure in the novel, represents something more than just a victim or love interest. Her transformation throughout the story reveals the fragility of innocence when confronted with evil. Her ability to resist Dracula’s influence, even as she is drawn closer to him, symbolizes the strength of human will and the power of love and friendship. It is her intellect, compassion, and the support of her companions that ultimately play a pivotal role in Dracula’s downfall. Mina’s character challenges the stereotypical victimized woman in gothic literature, as she not only survives but also becomes one of the strongest and most resilient figures in the fight against Dracula. \n\nThe male characters, especially Jonathan Harker, are tested by their journey into Dracula’s world. Harker, who initially seeks adventure and romance, quickly realizes the terrifying truth about his situation. His journals document not just the physical horror he faces but also the psychological toll of being trapped in Dracula’s lair. His struggle to escape is both a literal and metaphorical journey toward freedom and survival. The camaraderie and bond shared by the group of characters as they work to defeat Dracula is one of the novel’s most powerful elements. The strength of their unity contrasts sharply with Dracula’s isolation, as he is portrayed as a solitary, ancient figure who is ultimately undone by the collective strength of human connection and resolve. \n\nOne of the novel’s most disturbing features is the symbolic power of blood. Dracula’s thirst for blood, and the act of feeding, is a central metaphor for the parasitic nature of evil and the corrupting influence of the vampire. Blood in Dracula represents not only the loss of life but the loss of autonomy and identity. The act of blood-drinking is not just physical; it is an intimate invasion, one that turns the victim into a mere shadow of themselves. The vampire’s bite is symbolic of the breakdown of moral and spiritual boundaries, and Dracula’s quest to transform others into vampires represents the spread of evil and the destruction of what makes someone human. \n\nAt its conclusion, Dracula offers a sobering reflection on the battle between light and dark, life and death. The pursuit of Dracula, while ultimately successful, is fraught with loss, and the victory is bittersweet. The final moments, in which Dracula meets his demise, emphasize that evil, even when defeated, leaves a permanent scar on the world. The legacy of Dracula’s evil lingers in the characters’ lives, reminding them—and readers—that evil never truly disappears. The conclusion reinforces the novel’s enduring message about the fragility of life, the importance of moral courage, and the constant need to confront the darkness that lurks both outside and within. \n\nUltimately, Dracula is more than just a horror novel. It is a reflection on the nature of good and evil, the dangers of unchecked power, and the fear of the unknown. Bram Stoker’s masterpiece has become a timeless work of gothic fiction, influencing countless adaptations and interpretations. Its exploration of desire, fear, and the supernatural continues to resonate with readers today, making Dracula a novel that stands as both a product of its time and an ever-relevant commentary on the darkness that exists within humanity. Its tale of terror, love, and sacrifice remains a chilling reminder of the eternal struggle between the forces of darkness and the light of human resilience.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8604),
                            ImageUrl = "https://miro.medium.com/v2/resize:fit:1000/1*3US2qS3OLXELxVA0AEVZxQ.jpeg",
                            Introduction = "A chilling exploration of darkness, fear, and the supernatural, Dracula by Bram Stoker unravels a tale of obsession, seduction, and the eternal battle between life and death.",
                            IsDeleted = false,
                            Title = "The Eternal Horror of Dracula: A Deep Dive into Gothic Terror",
                            Views = 0
                        },
                        new
                        {
                            Id = 7,
                            Content = "First published in 1962, A Clockwork Orange by Anthony Burgess is a provocative and unsettling work that explores the complex intersection of free will, violence, and the role of society in shaping individual behavior. Set in a dystopian future, the novel follows Alex, a young delinquent with a penchant for violence, and his journey from cruelty to a forced form of 'rehabilitation.' Through its unsettling narrative, Burgess invites readers to question the nature of morality and the boundaries of human freedom. \n\nThe novel’s central theme revolves around the concept of free will. Alex, the protagonist, is a violent teenager who delights in criminal activities, yet when he is arrested and subjected to an experimental treatment to eliminate his desire for violence, the novel shifts its focus to a deeper philosophical question: what does it mean to truly be human if free will is taken away? The treatment strips Alex of his autonomy, forcing him to exist as a ‘clockwork orange,’ a machine-like creature devoid of the ability to choose between good and evil. Burgess, through this stark transformation, forces readers to grapple with the ethical question: Is it better to choose evil than to be deprived of the ability to choose at all? \n\nThe novel is known for its distinctive use of Nadsat, a fictional language created by Burgess, which blends Russian slang and English to give the novel a unique and disorienting atmosphere. The language serves as both a tool of alienation and a means of immersing the reader in Alex's world. Nadsat is central to the novel's tone, and its use emphasizes the youthful rebellion of the characters, as well as the gap between generations. It creates a sense of disconnection and unease, reflecting the novel's broader themes of societal breakdown and the alienation of the individual. \n\nViolence is a key element throughout A Clockwork Orange, not only as an action but as a social commentary. Burgess portrays a world in which violence has become normalized, whether through the actions of Alex and his friends or through the oppressive methods of state control. The novel doesn't shy away from the brutality of its characters' actions, including graphic scenes of murder, assault, and psychological torment. These scenes are designed not to shock for shock's sake, but to underline the pervasive violence in the world Burgess creates—a world where violence is both a tool of rebellion and a tool of control. \n\nThe dystopian society in A Clockwork Orange is one where the government attempts to manipulate and control its citizens in the name of societal order. Alex's forced treatment, known as Ludovico's Technique, is an extreme measure of government intervention aimed at rehabilitating criminals through psychological conditioning. However, the treatment raises the question of whether it is ethical to suppress an individual's free will, even if it is in the name of societal good. Through the protagonist’s experiences, Burgess critiques a society where the state has too much power over its citizens, and the individual’s autonomy is sacrificed for the illusion of peace. \n\nAt its heart, A Clockwork Orange is a novel about the tension between human freedom and societal control. The novel examines the implications of taking away an individual's ability to choose, forcing readers to question whether a life without free will is truly worth living. Is it more humane to allow someone to make their own choices, even if they are harmful, or is it better to impose conformity through control? The novel does not offer easy answers but instead leaves readers to grapple with the complex interplay of morality, freedom, and control. \n\nOne of the most compelling aspects of A Clockwork Orange is its exploration of the idea of redemption. Although Alex is subjected to a brutal treatment, the novel ultimately hints at the possibility of his redemption. In the final chapters, Alex begins to express a desire to leave his violent past behind, suggesting that even in a world filled with violence and corruption, there is room for change and growth. The final lines of the novel reflect Alex’s yearning for a better life, a future beyond his criminal past, and his ultimate choice to embrace maturity. \n\nA Clockwork Orange is as much a work of philosophy as it is of literature. It’s a fierce critique of the systems of control and authority that dominate society, but it’s also a profound meditation on the nature of free will and the human condition. Through Alex’s journey, Burgess poses difficult questions about the right to choose and the consequences of living in a world that seeks to eliminate choice. \n\nThe novel’s conclusion, with Alex’s eventual maturation and recognition of the need for change, serves as a reminder of the importance of personal agency. Despite the society around him, Alex realizes that true freedom lies in the ability to choose a different path, a reflection of hope even in the darkest of worlds. In the end, A Clockwork Orange remains a challenging and controversial work, but one that leaves a lasting impact on its readers, compelling them to think critically about violence, free will, and the role of society in shaping human behavior. \n\nA Clockwork Orange continues to resonate today as a powerful exploration of violence, control, and the power of individual choice. Its controversial themes, innovative language, and unforgettable characters have made it a defining work in the dystopian genre, ensuring its place as one of the most influential novels of the 20th century.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8606),
                            ImageUrl = "https://portsmouthreview.com/wp-content/uploads/2019/01/clockworkorange-1160x774.png",
                            Introduction = "A dark and thought-provoking exploration of free will, violence, and societal control, A Clockwork Orange by Anthony Burgess challenges perceptions of morality and human nature.",
                            IsDeleted = false,
                            Title = "The Dystopian Horror of A Clockwork Orange: A Deep Dive into Violence and Free Will",
                            Views = 0
                        },
                        new
                        {
                            Id = 8,
                            Content = "First published in 1934, Murder on the Orient Express is one of Agatha Christie’s most celebrated novels. The story unfolds aboard the famous Orient Express train, where an American millionaire, Samuel Ratchett, is found murdered in his compartment. The Belgian detective Hercule Poirot, who is traveling on the same train, is called upon to investigate the crime. As Poirot conducts his investigation, he uncovers a tangled web of lies and hidden secrets, with each passenger harboring a possible motive. \n\nThe novel is a brilliant example of Christie's skill in misdirection and plotting. Through her use of red herrings, multiple perspectives, and a carefully constructed set of clues, Christie keeps the reader guessing until the very end. The structure of the narrative, with Poirot’s methodical examination of the passengers and their testimonies, allows for a deep dive into human psychology and the complexities of moral judgment. \n\nAt its core, Murder on the Orient Express explores themes of justice and revenge. Throughout the story, Poirot is faced with difficult questions about what constitutes true justice and whether revenge is ever justified. As the mystery unravels, Poirot is forced to confront the moral ambiguity of the case, as he discovers that the passengers may not be what they initially appear to be. \n\nThe book's plot twist, revealed in the final pages, is one of the most famous in literary history. Christie’s ability to deceive and surprise her readers with such a shocking conclusion is a testament to her mastery of the genre. The revelation forces readers to reconsider the entire narrative and question the concept of justice and fairness.\n\nIn addition to its plot, Murder on the Orient Express is filled with rich characterizations. Poirot, known for his meticulous nature and sharp intellect, is presented as both a compassionate and calculating figure, contrasting with the colorful array of passengers aboard the train. The novel also features a variety of personalities, from the aristocratic to the common folk, each adding depth to the story and providing potential suspects for Poirot’s investigation.\n\nMurder on the Orient Express has remained a beloved classic for decades, inspiring numerous adaptations in film, television, and theater. Its themes of morality, justice, and revenge continue to resonate with readers, making it an essential work in the detective genre. Agatha Christie’s iconic writing and the unforgettable twists of this tale ensure that it remains a definitive example of the mystery novel.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8608),
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/81voh8NSRSL._AC_UL210_SR210,210_.jpg",
                            Introduction = "Agatha Christie’s Murder on the Orient Express is a classic detective novel that remains one of the most iconic works in the mystery genre. With Hercule Poirot at the helm, the story takes readers on a journey through the labyrinth of human motives, justice, and revenge, all set against the opulent backdrop of a luxurious train ride.",
                            IsDeleted = false,
                            Title = "Murder on the Orient Express: A Masterclass in Mystery and Morality",
                            Views = 0
                        },
                        new
                        {
                            Id = 9,
                            Content = "First published in 1937, Of Mice and Men is one of John Steinbeck's most famous works. Set against the backdrop of the Great Depression, the novel follows George Milton and Lennie Small, two migrant workers traveling through California in search of work. Despite their challenging lives, they share a common dream: to one day own a piece of land and live off it in peace.\n\nThe novel explores themes of loneliness, dreams, and human dignity. Steinbeck presents the harshness of the world through the lens of George and Lennie's relationship, highlighting the disparity between their hopes and the grim reality of their lives. Lennie, a large and mentally disabled man, depends on George for guidance and protection. George, in turn, takes on the responsibility of caring for Lennie, making their bond one of both friendship and survival.\n\nThrough a cast of other characters on the ranch, Steinbeck portrays the isolation and yearning for connection felt by many during the Great Depression. Characters like Candy, Crooks, and Curley's wife each grapple with their own desires for companionship and a better life, yet they are trapped by their circumstances.\n\nOf Mice and Men also examines the theme of powerlessness. The characters, particularly George and Lennie, are often powerless to change their fates due to the social and economic conditions of the time. Steinbeck uses their story to comment on the limitations of the American Dream, especially for marginalized individuals.\n\nThe novel’s tragic conclusion, with George making a heartbreaking decision about Lennie’s fate, serves as a powerful commentary on the human condition. It forces readers to confront the sacrifices individuals make for love, loyalty, and friendship, while also questioning the cost of these decisions.\n\nUltimately, Of Mice and Men is a tale about the fragility of human dreams and the deep need for companionship, making it a timeless classic that resonates with readers across generations. Steinbeck’s masterful storytelling and deep empathy for his characters ensure the novel’s place in the canon of American literature.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8611),
                            ImageUrl = "https://m.media-amazon.com/images/I/81nuvmpbEdL._AC_UF1000,1000_QL80_.jpg",
                            Introduction = "John Steinbeck's Of Mice and Men is a poignant tale of friendship, dreams, and the harsh realities of the American Depression. It chronicles the lives of two displaced migrant workers, George and Lennie, who dream of a better life, yet struggle against the societal and economic forces that limit their aspirations.",
                            IsDeleted = false,
                            Title = "Of Mice and Men: The Tragic Story of Friendship and Dreams",
                            Views = 0
                        },
                        new
                        {
                            Id = 10,
                            Content = "Published in 1965, Dune is considered one of the greatest works of science fiction ever written. Set on the desert planet of Arrakis, the novel follows Paul Atreides, a young nobleman whose family is entrusted with the stewardship of the planet and its spice production. The spice melange, which is essential for space travel and prolonging life, makes Arrakis the most important planet in the universe. As Paul’s family navigates the dangerous political landscape of the planet, they are drawn into a web of betrayal, rebellion, and prophecy.\n\nAt its core, Dune is a story about power, both its acquisition and its consequences. Paul Atreides is not only struggling with his family's political legacy but also with his own destiny. As he uncovers his latent abilities and learns of the prophecy surrounding him, he must decide what role he will play in the fate of Arrakis and its people.\n\nThe novel is also a profound exploration of ecology and environmentalism. Herbert presents the planet Arrakis as a harsh and unforgiving world, where water is scarce, and the survival of its inhabitants depends on their ability to adapt to the land. Through his vivid descriptions of the desert landscape and the Fremen people who inhabit it, Herbert conveys a deep respect for the delicate balance of ecosystems and the consequences of exploiting natural resources.\n\nOne of the most striking aspects of *Dune* is its intricate world-building. Herbert creates a universe with multiple layers of complexity, from the politics of the noble houses to the religious beliefs of the Fremen to the guild of Navigators who control space travel. The conflict between these factions, each with their own agenda, forms the backdrop of the novel's central drama. Herbert's keen insight into human nature and his ability to weave together these various strands of conflict make *Dune* a deeply engaging and thought-provoking read.\n\nHowever, Dune is not only about power and politics. The novel also examines themes of human evolution, mysticism, and the role of religion in shaping societies. Paul’s journey to embrace his destiny involves both physical and mental transformation, leading to questions about free will, fate, and the responsibilities of leadership.\n\nHerbert’s prose is rich and dense, and while some readers may find the novel’s depth challenging, its rewards are immense. The novel concludes with a climactic and unforgettable moment that sets the stage for the series’ subsequent books.\n\nUltimately, Dune is a tale of survival, leadership, and the burden of destiny. It is a novel that encourages readers to reflect on the power dynamics within our own societies and the ways in which human beings relate to the environment and to each other. Frank Herbert’s Dune continues to inspire readers and adaptations, solidifying its place as a landmark achievement in science fiction literature.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8678),
                            ImageUrl = "https://i.etsystatic.com/16102212/r/il/2482ff/2771009725/il_570xN.2771009725_chlb.jpg",
                            Introduction = "Frank Herbert's *Dune* is a monumental science fiction saga that explores the complex interplay of politics, religion, and ecology on the desert planet of Arrakis. The novel delves into the struggle for control of the universe's most valuable substance, the spice melange, while exploring themes of leadership, destiny, and environmentalism.",
                            IsDeleted = false,
                            Title = "Dune: The Epic Tale of Power, Ecology, and Human Destiny",
                            Views = 0
                        },
                        new
                        {
                            Id = 11,
                            Content = "First published in 1979, The Hitchhiker's Guide to the Galaxy is one of the most beloved and influential works of science fiction, known for its unique combination of comedy, satire, and cosmic adventure. The novel introduces readers to Arthur Dent, a regular, unassuming man who finds himself thrust into an absurd journey after Earth is destroyed to make way for a hyperspace bypass. Arthur is rescued by Ford Prefect, a researcher for the titular Guide, who takes him on a space-faring adventure filled with bizarre characters and strange situations.\n\nThe novel is filled with humor that both mocks and celebrates the absurdities of life. Adams' distinctive wit shines through in every chapter, as he presents the reader with outlandish scenarios and deadpan observations. The narrative is packed with tongue-in-cheek moments, from the eccentric Vogon bureaucrats to the two-headed, three-armed Zaphod Beeblebrox. The absurdity is paired with sharp social commentary, as Adams questions human existence, bureaucracy, and the nature of the universe.\n\nAt its core, The Hitchhiker's Guide to the Galaxy is a philosophical exploration of life, the universe, and everything—literally. The novel's famous phrase 'Don't Panic' is a tongue-in-cheek mantra that perfectly encapsulates the book’s view on life’s uncertainties. The story delves into questions about meaning, fate, and chance, all while keeping the reader laughing along the way.\n\nThe impact of The Hitchhiker's Guide to the Galaxy is immense. It has influenced generations of writers, comedians, and filmmakers, becoming a cornerstone of popular culture. Adams' work has been adapted into various formats, including radio shows, television series, stage plays, and a feature film. Despite these adaptations, the original book remains the definitive version of the story, beloved by fans for its ingenious blend of humor, science fiction, and philosophical exploration.\n\nIn conclusion, The Hitchhiker's Guide to the Galaxy is more than just a science fiction novel—it’s a literary treasure that encourages readers to laugh, think, and reflect on the absurdity of life. Its timeless humor and deep questions about existence continue to resonate with readers around the world, making it a must-read for fans of science fiction and comedy alike.",
                            CreatedOn = new DateTime(2025, 3, 21, 13, 45, 34, 567, DateTimeKind.Local).AddTicks(8681),
                            ImageUrl = "https://cdn2.penguin.com.au/covers/original/9780434023394.jpg",
                            Introduction = "Douglas Adams' The Hitchhiker's Guide to the Galaxy is a classic blend of science fiction, absurdity, and humor. The story follows Arthur Dent as he is suddenly swept off Earth just before its destruction, leading to a wild and philosophical adventure through space.",
                            IsDeleted = false,
                            Title = "The Hitchhiker's Guide to the Galaxy: A Journey Through Absurdity and Wit",
                            Views = 0
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Authors.Data.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageRating")
                        .HasColumnType("float");

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BornAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("NationalityId")
                        .HasColumnType("int");

                    b.Property<string>("PenName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("RatingsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("NationalityId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AverageRating = 4.7000000000000002,
                            Biography = "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which have been adapted into feature films, miniseries, television series, and comic books. Notable works include Carrie, The Shining, IT, Misery, Pet Sematary, and The Dark Tower series. King is renowned for his ability to create compelling, complex characters and for his mastery of building suspenseful, intricately woven narratives. Aside from his novels, he has written nearly 200 short stories, most of which have been compiled into collections such as Night Shift, Skeleton Crew, and Everything's Eventual. He has received numerous awards for his contributions to literature, including the National Book Foundation's Medal for Distinguished Contribution to American Letters in 2003. King often writes under the pen name 'Richard Bachman,' a pseudonym he used to publish early works such as Rage and The Long Walk. He lives in Bangor, Maine, with his wife, fellow novelist Tabitha King, and continues to write and inspire new generations of readers and writers.",
                            BornAt = new DateTime(1947, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Stephen King",
                            NationalityId = 182,
                            PenName = "Richard Bachman",
                            RatingsCount = 14
                        },
                        new
                        {
                            Id = 2,
                            AverageRating = 4.75,
                            Biography = "Joanne Rowling (born July 31, 1965), known by her pen name J.K. Rowling, is a British author and philanthropist. She is best known for writing the Harry Potter series, a seven-book fantasy saga that has become a global phenomenon. The series has sold over 600 million copies, been translated into 84 languages, and inspired a massive multimedia franchise, including blockbuster films, stage plays, video games, and theme parks. Notable works include Harry Potter and the Philosopher's Stone, Harry Potter and the Deathly Hallows, and The Tales of Beedle the Bard. Rowling's writing is praised for its imaginative world-building, compelling characters, and exploration of themes such as love, loyalty, and the battle between good and evil. After completing the Harry Potter series, Rowling transitioned to writing for adults, debuting with The Casual Vacancy, a contemporary social satire. She also writes crime fiction under the pseudonym Robert Galbraith, authoring the acclaimed Cormoran Strike series. Rowling has received numerous awards and honors for her literary achievements, including the Order of the British Empire (OBE) for services to children’s literature. She is an advocate for various charitable causes and founded the Volant Charitable Trust to combat social inequality. Rowling lives in Scotland with her family and continues to write, inspiring readers of all ages with her imaginative storytelling and philanthropy.",
                            BornAt = new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 1,
                            ImageUrl = "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Joanne Rowling",
                            NationalityId = 181,
                            PenName = "J. K. Rowling",
                            RatingsCount = 4
                        },
                        new
                        {
                            Id = 3,
                            AverageRating = 4.6699999999999999,
                            Biography = "John Ronald Reuel Tolkien (January 3, 1892 – September 2, 1973) was an English writer, philologist, and academic. He is best known as the author of The Hobbit and The Lord of the Rings, two of the most beloved works in modern fantasy literature. Tolkien's work has had a profound impact on the fantasy genre, establishing many of the conventions and archetypes that define it today. Set in the richly detailed world of Middle-earth, his stories feature intricate mythologies, languages, and histories, reflecting his scholarly expertise in philology and medieval studies. The Hobbit (1937) was a critical and commercial success, leading to the epic sequel The Lord of the Rings trilogy (1954–1955), which has sold over 150 million copies and been adapted into award-winning films by director Peter Jackson. Tolkien also authored The Silmarillion, a collection of myths and legends that expand the lore of Middle-earth. He served as a professor of Anglo-Saxon at the University of Oxford, where he was a member of the literary group The Inklings, alongside C.S. Lewis. Tolkien's contributions to literature earned him global acclaim and a lasting legacy as the 'father of modern fantasy.' He passed away in 1973, but his works continue to captivate readers worldwide.",
                            BornAt = new DateTime(1892, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(1973, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "John Ronald Reuel Tolkien ",
                            NationalityId = 181,
                            PenName = "J.R.R Tolkien",
                            RatingsCount = 6
                        },
                        new
                        {
                            Id = 4,
                            AverageRating = 4.5999999999999996,
                            Biography = "Ken Elton Kesey (September 17, 1935 – November 10, 2001) was an American novelist, essayist, and countercultural figure. He is best known for his debut novel One Flew Over the Cuckoo's Nest (1962), which explores themes of individuality, authority, and mental health. The novel became an instant classic, earning critical acclaim for its portrayal of life inside a psychiatric institution and inspiring a celebrated 1975 film adaptation starring Jack Nicholson that won five Academy Awards. Kesey was deeply involved in the counterculture movement of the 1960s, becoming a key figure among the Merry Pranksters, a group famous for their cross-country bus trip chronicled in Tom Wolfe's The Electric Kool-Aid Acid Test. His second novel, Sometimes a Great Notion (1964), was praised for its ambitious narrative structure and portrayal of family dynamics. Kesey's work often reflects his fascination with the human condition, rebellion, and the nature of freedom. In addition to his literary career, Kesey experimented with psychedelics, drawing inspiration from his participation in government experiments and his own personal experiences. He remained an influential voice in American literature and culture, inspiring generations of readers and writers to question societal norms and explore new perspectives. Kesey passed away in 2001, leaving behind a legacy of bold storytelling and a spirit of rebellion.",
                            BornAt = new DateTime(1935, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(2001, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/9/9b/Ken_Kesey%2C_American_author%2C_1935-2001.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Ken Kesey",
                            NationalityId = 182,
                            RatingsCount = 6
                        },
                        new
                        {
                            Id = 5,
                            AverageRating = 4.9000000000000004,
                            Biography = "Eric Arthur Blair (June 25, 1903 – January 21, 1950), known by his pen name George Orwell, was an English novelist, essayist, journalist, and critic. Orwell is best known for his works Animal Farm (1945) and Nineteen Eighty-Four (1949), both of which are considered cornerstones of modern English literature and have had a lasting impact on political thought. Animal Farm, an allegorical novella satirizing the Russian Revolution and the rise of Stalinism, has become a standard in literature about totalitarianism and political corruption. Nineteen Eighty-Four, a dystopian novel set in a totalitarian society controlled by Big Brother, has influenced political discourse around surveillance, propaganda, and government control. Orwell's writings often sexplore themes of social injustice, totalitarianism, and the misuse of power. He was an ardent critic of fascism and communism and was deeply involved in political activism, including fighting in the Spanish Civil War, which influenced his strong anti-authoritarian views. Orwell's style is known for its clarity, precision, and biting social commentary, and his work continues to be relevant and influential in discussions on politics, language, and individual rights. Orwell passed away in 1950 at the age of 46, but his work remains widely read and influential, especially in the context of discussions surrounding state power, civil liberties, and the role of the individual in society.",
                            BornAt = new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(1950, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/george-orwell.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "George Orwell",
                            NationalityId = 181,
                            PenName = "George Orwell",
                            RatingsCount = 7
                        },
                        new
                        {
                            Id = 6,
                            AverageRating = 4.5999999999999996,
                            Biography = "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula, which has become a classic of literature and a foundational work in the vampire genre. Born in Dublin, Ireland, Stoker was a writer, theater manager, and journalist. While Dracula is his most famous work, Stoker wrote numerous other novels, short stories, and essays. He was greatly influenced by Gothic fiction and folklore, particularly Eastern European myths about vampires, which he drew upon to create his iconic antagonist, Count Dracula. Stoker's Dracula introduced the figure of the vampire into mainstream literature and has inspired countless adaptations in film, theater, and popular culture. Though Stoker's works initially received mixed critical reception, Dracula gained increasing recognition and has had a lasting impact on the horror genre. Stoker's writing was often characterized by vivid, atmospheric descriptions and psychological complexity, and his portrayal of fear, desire, and the supernatural remains influential today. Stoker passed away in 1912 at the age of 64, but his legacy as the creator of one of literature's most enduring villains continues to captivate readers around the world.",
                            BornAt = new DateTime(1847, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(1912, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Bram Stoker",
                            NationalityId = 78,
                            PenName = "Bram Stoker",
                            RatingsCount = 6
                        },
                        new
                        {
                            Id = 7,
                            AverageRating = 4.2999999999999998,
                            Biography = "John Anthony Burgess Wilson (February 25, 1917 – November 25, 1993) was an English author, composer, and linguist, best known for his dystopian novel A Clockwork Orange. Born in Manchester, England, Burgess worked in a variety of fields before becoming a full-time writer. His work often explores themes of free will, violence, and social control. A Clockwork Orange, published in 1962, became a landmark work in the genre of dystopian fiction, and the controversial novel was later adapted into a famous film by Stanley Kubrick. Burgess was also known for his deep knowledge of language and for his ability to create complex, linguistically rich narratives. Throughout his career, Burgess wrote numerous novels, short stories, plays, and essays, in addition to works of music and translations. His works, while not always critically lauded, gained a loyal following, and he is considered an influential figure in British literature. Burgess continued to write prolifically until his death in 1993 at the age of 76.",
                            BornAt = new DateTime(1917, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(1993, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://grahamholderness.com/wp-content/uploads/2019/04/anthony-burgess-at-home-i-009.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Anthony Burgess",
                            NationalityId = 181,
                            PenName = "Anthony Burgess",
                            RatingsCount = 6
                        },
                        new
                        {
                            Id = 8,
                            AverageRating = 4.4800000000000004,
                            Biography = "Agatha Mary Clarissa Christie (September 15, 1890 – January 12, 1976) was an English writer, widely recognized for her detective fiction. She is best known for creating iconic characters such as Hercule Poirot and Miss Marple, who became central figures in numerous detective novels, short stories, and plays. Christie’s writing is characterized by intricate plots, psychological depth, and unexpected twists. Her first novel, The Mysterious Affair at Styles, introduced the world to Hercule Poirot in 1920. Over the following decades, Christie would become the best-selling novelist of all time, with over two billion copies of her books sold worldwide. Her works have been translated into over 100 languages, and her influence on the mystery genre is profound. Christie was appointed a Dame Commander of the Order of the British Empire in 1971. She continued writing until her death in 1976, leaving behind a legacy that includes Murder on the Orient Express, Death on the Nile, and And Then There Were None.",
                            BornAt = new DateTime(1890, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(1976, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 1,
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/cf/Agatha_Christie.png",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Agatha Christie",
                            NationalityId = 181,
                            PenName = "Agatha Christie",
                            RatingsCount = 7
                        },
                        new
                        {
                            Id = 9,
                            AverageRating = 4.5,
                            Biography = "John Ernst Steinbeck Jr. (February 27, 1902 – December 20, 1968) was an American author known for his impactful portrayal of the struggles of the working class and the disenfranchised. His novels, such as 'The Grapes of Wrath', 'Of Mice and Men', and 'East of Eden', often explored themes of poverty, social injustice, and the human condition. Steinbeck won the Nobel Prize for Literature in 1962 for his realistic and imaginative writings, which had a lasting effect on American literature. Born in Salinas, California, Steinbeck's early works depicted life during the Great Depression, while his later novels explored more personal and philosophical themes. He is best remembered for his compassion for ordinary people and his insightful critiques of social systems. His writing style was marked by his use of vivid, poetic descriptions and a deep understanding of his characters' struggles.",
                            BornAt = new DateTime(1902, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(1968, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://m.media-amazon.com/images/M/MV5BYzBiNmIxOWYtNTc3NS00OTg4LTgxOGYtOTJmYTk3NzJhYzE1XkEyXkFqcGc@._V1_.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "John Steinbeck",
                            NationalityId = 182,
                            PenName = "John Steinbeck",
                            RatingsCount = 4
                        },
                        new
                        {
                            Id = 10,
                            AverageRating = 4.6699999999999999,
                            Biography = "Frank Herbert (October 8, 1920 – February 11, 1986) was an American science fiction author best known for his landmark series Dune. Herbert's work frequently explored themes of politics, religion, and ecology. *Dune*, first published in 1965, remains one of the most influential and best-selling science fiction novels of all time, winning the Hugo and Nebula Awards. Born in Tacoma, Washington, Herbert began his writing career as a journalist, before transitioning to fiction writing. His Dune series, which consists of six novels, delves into the complexities of power, governance, environmentalism, and the human condition. Herbert's distinctive writing style is known for its philosophical depth, intricate world-building, and the exploration of human evolution. His work has had a profound influence on both science fiction and popular culture. Herbert passed away in 1986, but his legacy continues through his novels, which are still widely read and respected today.",
                            BornAt = new DateTime(1920, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(1986, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://www.historylink.org/Content/Media/Photos/Large/Frank-Herbert-signing-books-Seattle-December-5-1971.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Frank Herbert",
                            NationalityId = 182,
                            PenName = "Frank Herbert",
                            RatingsCount = 3
                        },
                        new
                        {
                            Id = 11,
                            AverageRating = 4.5,
                            Biography = "Douglas Adams (March 11, 1952 – May 11, 2001) was an English author, best known for his science fiction series The Hitchhiker's Guide to the Galaxy. Adams’ works are renowned for their wit, absurdity, and philosophical depth. Born in Cambridge, England, Adams began his career in comedy writing before transitioning into fiction. The Hitchhiker's Guide to the Galaxy, first published as a radio play in 1978 and later as a novel in 1979, became a cultural phenomenon, blending humor, satire, and sci-fi elements into a beloved classic. The series, which spans multiple books, explores themes of existence, the absurdity of life, and the universe in a way that resonates with both fans of science fiction and general readers. Adams’ distinctive voice and irreverent humor continue to inspire and entertain readers today. He also worked as a scriptwriter and wrote other notable works, including Dirk Gently's Holistic Detective Agency. Adams passed away in 2001 at the age of 49, but his influence on the science fiction genre and popular culture remains enduring.",
                            BornAt = new DateTime(1952, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiedAt = new DateTime(2001, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            ImageUrl = "https://static.tvtropes.org/pmwiki/pub/images/DouglasAdams_douglasadams_com.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Douglas Adams",
                            NationalityId = 181,
                            PenName = "Douglas Adams",
                            RatingsCount = 4
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Authors.Data.Models.Nationality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Nationalities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Afghanistan"
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Albania"
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Algeria"
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Andorra"
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Angola"
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Antigua and Barbuda"
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Argentina"
                        },
                        new
                        {
                            Id = 8,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Armenia"
                        },
                        new
                        {
                            Id = 9,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Australia"
                        },
                        new
                        {
                            Id = 10,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Austria"
                        },
                        new
                        {
                            Id = 11,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Azerbaijan"
                        },
                        new
                        {
                            Id = 12,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bahamas"
                        },
                        new
                        {
                            Id = 13,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bahrain"
                        },
                        new
                        {
                            Id = 14,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bangladesh"
                        },
                        new
                        {
                            Id = 15,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Barbados"
                        },
                        new
                        {
                            Id = 16,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Belarus"
                        },
                        new
                        {
                            Id = 17,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Belgium"
                        },
                        new
                        {
                            Id = 18,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Belize"
                        },
                        new
                        {
                            Id = 19,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Benin"
                        },
                        new
                        {
                            Id = 20,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bhutan"
                        },
                        new
                        {
                            Id = 21,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bolivia"
                        },
                        new
                        {
                            Id = 22,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bosnia and Herzegovina"
                        },
                        new
                        {
                            Id = 23,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Botswana"
                        },
                        new
                        {
                            Id = 24,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Brazil"
                        },
                        new
                        {
                            Id = 25,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Brunei"
                        },
                        new
                        {
                            Id = 26,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bulgaria"
                        },
                        new
                        {
                            Id = 27,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Burkina Faso"
                        },
                        new
                        {
                            Id = 28,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Burundi"
                        },
                        new
                        {
                            Id = 29,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cabo Verde"
                        },
                        new
                        {
                            Id = 30,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cambodia"
                        },
                        new
                        {
                            Id = 31,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cameroon"
                        },
                        new
                        {
                            Id = 32,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Canada"
                        },
                        new
                        {
                            Id = 33,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Central African Republic"
                        },
                        new
                        {
                            Id = 34,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chad"
                        },
                        new
                        {
                            Id = 35,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chile"
                        },
                        new
                        {
                            Id = 36,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "China"
                        },
                        new
                        {
                            Id = 37,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Colombia"
                        },
                        new
                        {
                            Id = 38,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Comoros"
                        },
                        new
                        {
                            Id = 39,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Costa Rica"
                        },
                        new
                        {
                            Id = 40,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Croatia"
                        },
                        new
                        {
                            Id = 41,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cuba"
                        },
                        new
                        {
                            Id = 42,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cyprus"
                        },
                        new
                        {
                            Id = 43,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Czech Republic"
                        },
                        new
                        {
                            Id = 44,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Denmark"
                        },
                        new
                        {
                            Id = 45,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Djibouti"
                        },
                        new
                        {
                            Id = 46,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Dominica"
                        },
                        new
                        {
                            Id = 47,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Dominican Republic"
                        },
                        new
                        {
                            Id = 48,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ecuador"
                        },
                        new
                        {
                            Id = 49,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Egypt"
                        },
                        new
                        {
                            Id = 50,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "El Salvador"
                        },
                        new
                        {
                            Id = 51,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Equatorial Guinea"
                        },
                        new
                        {
                            Id = 52,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Eritrea"
                        },
                        new
                        {
                            Id = 53,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Estonia"
                        },
                        new
                        {
                            Id = 54,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Eswatini"
                        },
                        new
                        {
                            Id = 55,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ethiopia"
                        },
                        new
                        {
                            Id = 56,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Fiji"
                        },
                        new
                        {
                            Id = 57,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Finland"
                        },
                        new
                        {
                            Id = 58,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "France"
                        },
                        new
                        {
                            Id = 59,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Gabon"
                        },
                        new
                        {
                            Id = 60,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Gambia"
                        },
                        new
                        {
                            Id = 61,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Georgia"
                        },
                        new
                        {
                            Id = 62,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Germany"
                        },
                        new
                        {
                            Id = 63,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ghana"
                        },
                        new
                        {
                            Id = 64,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Greece"
                        },
                        new
                        {
                            Id = 65,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Grenada"
                        },
                        new
                        {
                            Id = 66,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guatemala"
                        },
                        new
                        {
                            Id = 67,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guinea"
                        },
                        new
                        {
                            Id = 68,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guinea-Bissau"
                        },
                        new
                        {
                            Id = 69,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Guyana"
                        },
                        new
                        {
                            Id = 70,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Haiti"
                        },
                        new
                        {
                            Id = 71,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Honduras"
                        },
                        new
                        {
                            Id = 72,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Hungary"
                        },
                        new
                        {
                            Id = 73,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Iceland"
                        },
                        new
                        {
                            Id = 74,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "India"
                        },
                        new
                        {
                            Id = 75,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Indonesia"
                        },
                        new
                        {
                            Id = 76,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Iran"
                        },
                        new
                        {
                            Id = 77,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Iraq"
                        },
                        new
                        {
                            Id = 78,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ireland"
                        },
                        new
                        {
                            Id = 79,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Israel"
                        },
                        new
                        {
                            Id = 80,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Italy"
                        },
                        new
                        {
                            Id = 81,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Jamaica"
                        },
                        new
                        {
                            Id = 82,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Japan"
                        },
                        new
                        {
                            Id = 83,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Jordan"
                        },
                        new
                        {
                            Id = 84,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kazakhstan"
                        },
                        new
                        {
                            Id = 85,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kenya"
                        },
                        new
                        {
                            Id = 86,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kiribati"
                        },
                        new
                        {
                            Id = 87,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "North Korea"
                        },
                        new
                        {
                            Id = 88,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "South Korea"
                        },
                        new
                        {
                            Id = 89,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kuwait"
                        },
                        new
                        {
                            Id = 90,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Kyrgyzstan"
                        },
                        new
                        {
                            Id = 91,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Laos"
                        },
                        new
                        {
                            Id = 92,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Latvia"
                        },
                        new
                        {
                            Id = 93,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Lebanon"
                        },
                        new
                        {
                            Id = 94,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Lesotho"
                        },
                        new
                        {
                            Id = 95,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Liberia"
                        },
                        new
                        {
                            Id = 96,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Libya"
                        },
                        new
                        {
                            Id = 97,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Liechtenstein"
                        },
                        new
                        {
                            Id = 98,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Lithuania"
                        },
                        new
                        {
                            Id = 99,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Luxembourg"
                        },
                        new
                        {
                            Id = 100,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Madagascar"
                        },
                        new
                        {
                            Id = 101,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Malawi"
                        },
                        new
                        {
                            Id = 102,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Malaysia"
                        },
                        new
                        {
                            Id = 103,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Maldives"
                        },
                        new
                        {
                            Id = 104,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mali"
                        },
                        new
                        {
                            Id = 105,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Malta"
                        },
                        new
                        {
                            Id = 106,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Marshall Islands"
                        },
                        new
                        {
                            Id = 107,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mauritania"
                        },
                        new
                        {
                            Id = 108,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mauritius"
                        },
                        new
                        {
                            Id = 109,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mexico"
                        },
                        new
                        {
                            Id = 110,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Micronesia"
                        },
                        new
                        {
                            Id = 111,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Moldova"
                        },
                        new
                        {
                            Id = 112,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Monaco"
                        },
                        new
                        {
                            Id = 113,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mongolia"
                        },
                        new
                        {
                            Id = 114,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Montenegro"
                        },
                        new
                        {
                            Id = 115,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Morocco"
                        },
                        new
                        {
                            Id = 116,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mozambique"
                        },
                        new
                        {
                            Id = 117,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Myanmar"
                        },
                        new
                        {
                            Id = 118,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Namibia"
                        },
                        new
                        {
                            Id = 119,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nauru"
                        },
                        new
                        {
                            Id = 120,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nepal"
                        },
                        new
                        {
                            Id = 121,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Netherlands"
                        },
                        new
                        {
                            Id = 122,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "New Zealand"
                        },
                        new
                        {
                            Id = 123,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nicaragua"
                        },
                        new
                        {
                            Id = 124,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Niger"
                        },
                        new
                        {
                            Id = 125,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Nigeria"
                        },
                        new
                        {
                            Id = 126,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "North Macedonia"
                        },
                        new
                        {
                            Id = 127,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Norway"
                        },
                        new
                        {
                            Id = 128,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Oman"
                        },
                        new
                        {
                            Id = 129,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Pakistan"
                        },
                        new
                        {
                            Id = 130,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Palau"
                        },
                        new
                        {
                            Id = 131,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Panama"
                        },
                        new
                        {
                            Id = 132,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Papua New Guinea"
                        },
                        new
                        {
                            Id = 133,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Paraguay"
                        },
                        new
                        {
                            Id = 134,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Peru"
                        },
                        new
                        {
                            Id = 135,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Philippines"
                        },
                        new
                        {
                            Id = 136,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Poland"
                        },
                        new
                        {
                            Id = 137,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Portugal"
                        },
                        new
                        {
                            Id = 138,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Qatar"
                        },
                        new
                        {
                            Id = 139,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Romania"
                        },
                        new
                        {
                            Id = 140,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Russia"
                        },
                        new
                        {
                            Id = 141,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Rwanda"
                        },
                        new
                        {
                            Id = 142,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saint Kitts and Nevis"
                        },
                        new
                        {
                            Id = 143,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saint Lucia"
                        },
                        new
                        {
                            Id = 144,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saint Vincent and the Grenadines"
                        },
                        new
                        {
                            Id = 145,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Samoa"
                        },
                        new
                        {
                            Id = 146,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "San Marino"
                        },
                        new
                        {
                            Id = 147,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "São Tomé and Príncipe"
                        },
                        new
                        {
                            Id = 148,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Saudi Arabia"
                        },
                        new
                        {
                            Id = 149,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Senegal"
                        },
                        new
                        {
                            Id = 150,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Serbia"
                        },
                        new
                        {
                            Id = 151,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Seychelles"
                        },
                        new
                        {
                            Id = 152,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sierra Leone"
                        },
                        new
                        {
                            Id = 153,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Singapore"
                        },
                        new
                        {
                            Id = 154,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Slovakia"
                        },
                        new
                        {
                            Id = 155,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Slovenia"
                        },
                        new
                        {
                            Id = 156,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Solomon Islands"
                        },
                        new
                        {
                            Id = 157,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Somalia"
                        },
                        new
                        {
                            Id = 158,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "South Africa"
                        },
                        new
                        {
                            Id = 159,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "South Sudan"
                        },
                        new
                        {
                            Id = 160,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Spain"
                        },
                        new
                        {
                            Id = 161,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sri Lanka"
                        },
                        new
                        {
                            Id = 162,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sudan"
                        },
                        new
                        {
                            Id = 163,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Suriname"
                        },
                        new
                        {
                            Id = 164,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sweden"
                        },
                        new
                        {
                            Id = 165,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Switzerland"
                        },
                        new
                        {
                            Id = 166,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Syria"
                        },
                        new
                        {
                            Id = 167,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Taiwan"
                        },
                        new
                        {
                            Id = 168,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tajikistan"
                        },
                        new
                        {
                            Id = 169,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tanzania"
                        },
                        new
                        {
                            Id = 170,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Thailand"
                        },
                        new
                        {
                            Id = 171,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Togo"
                        },
                        new
                        {
                            Id = 172,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tonga"
                        },
                        new
                        {
                            Id = 173,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Trinidad and Tobago"
                        },
                        new
                        {
                            Id = 174,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tunisia"
                        },
                        new
                        {
                            Id = 175,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Turkey"
                        },
                        new
                        {
                            Id = 176,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Turkmenistan"
                        },
                        new
                        {
                            Id = 177,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Tuvalu"
                        },
                        new
                        {
                            Id = 178,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Uganda"
                        },
                        new
                        {
                            Id = 179,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Ukraine"
                        },
                        new
                        {
                            Id = 180,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "United Arab Emirates"
                        },
                        new
                        {
                            Id = 181,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "United Kingdom"
                        },
                        new
                        {
                            Id = 182,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "United States"
                        },
                        new
                        {
                            Id = 183,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Unknown"
                        },
                        new
                        {
                            Id = 184,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Uruguay"
                        },
                        new
                        {
                            Id = 185,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Uzbekistan"
                        },
                        new
                        {
                            Id = 186,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vanuatu"
                        },
                        new
                        {
                            Id = 187,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vatican City"
                        },
                        new
                        {
                            Id = 188,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Venezuela"
                        },
                        new
                        {
                            Id = 189,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vietnam"
                        },
                        new
                        {
                            Id = 190,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Yemen"
                        },
                        new
                        {
                            Id = 191,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Zambia"
                        },
                        new
                        {
                            Id = 192,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Zimbabwe"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Book.Data.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<double>("AverageRating")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LongDescription")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RatingsCount")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            AverageRating = 4.25,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Pet Sematary is a 1983 horror novel by American writer Stephen King. The story revolves around the Creed family who move into a rural home near a pet cemetery that has the power to resurrect the dead. However, the resurrected creatures return with sinister changes. The novel explores themes of grief, mortality, and the consequences of tampering with nature. It was nominated for a World Fantasy Award for Best Novel in 1984. The book was adapted into two films: one in 1989 directed by Mary Lambert and another in 2019 directed by Kevin Kölsch and Dennis Widmyer. In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition, further solidifying its status as a classic in horror literature.",
                            PublishedDate = new DateTime(1983, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "Sometimes dead is better.",
                            Title = "Pet Sematary"
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 2,
                            AverageRating = 4.75,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Harry Potter and the Deathly Hallows is a fantasy novel written by British author J.K. Rowling. It is the seventh and final book in the Harry Potter series and concludes the epic tale of Harry's battle against Lord Voldemort. The story begins with Harry, Hermione, and Ron embarking on a dangerous quest to locate and destroy Voldemort's Horcruxes, which are key to his immortality. Along the way, they uncover secrets about the Deathly Hallows—three powerful magical objects that could aid them in their fight. The book builds to an intense and emotional climax at the Battle of Hogwarts, where Harry confronts Voldemort for the last time. Released on 21 July 2007, the book became a cultural phenomenon, breaking sales records and receiving critical acclaim for its complex characters, intricate plotting, and resonant themes of sacrifice, friendship, and love.",
                            PublishedDate = new DateTime(2007, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "The last book from the Harry Potter series.",
                            Title = "Harry Potter and the Deathly Hallows"
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 3,
                            AverageRating = 4.6699999999999999,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "The Lord of the Rings is a high-fantasy novel written by J.R.R. Tolkien and set in the fictional world of Middle-earth. The story follows the journey of Frodo Baggins, a humble hobbit who inherits the One Ring—a powerful artifact created by the Dark Lord Sauron to control Middle-earth. Along with a fellowship of companions, Frodo sets out on a perilous mission to destroy the ring in the fires of Mount Doom, the only place where it can be unmade. The narrative interweaves themes of friendship, courage, sacrifice, and the corrupting influence of power. Written in stages between 1937 and 1949, the novel is widely regarded as one of the greatest works of fantasy literature, influencing countless authors and spawning adaptations, including Peter Jackson's acclaimed film trilogy. With over 150 million copies sold, it remains one of the best-selling books of all time, praised for its richly detailed world-building, complex characters, and timeless appeal.",
                            PublishedDate = new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 6,
                            ShortDescription = "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.",
                            Title = "Lord of the Rings"
                        },
                        new
                        {
                            Id = 4,
                            AuthorId = 5,
                            AverageRating = 4.7999999999999998,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1327144697i/3744438.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "1984, written by George Orwell, is a dystopian novel set in a totalitarian society under the omnipresent surveillance of Big Brother. Published in 1949, the story follows Winston Smith, a low-ranking member of the Party, as he secretly rebels against the oppressive regime. Through his illicit love affair with Julia and his pursuit of forbidden knowledge, Winston challenges the Party's control over truth, history, and individuality. The novel introduces concepts such as 'doublethink,' 'Newspeak,' and 'thoughtcrime,' which have since become part of modern political discourse. Widely regarded as a classic of English literature, 1984 is a chilling exploration of propaganda, censorship, and the erosion of personal freedoms, serving as a cautionary tale for future generations.",
                            PublishedDate = new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 5,
                            ShortDescription = "A dystopian novel exploring the dangers of totalitarianism.",
                            Title = "1984"
                        },
                        new
                        {
                            Id = 5,
                            AuthorId = 4,
                            AverageRating = 4.8300000000000001,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://m.media-amazon.com/images/I/61Lpsc7B3jL.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "One Flew Over the Cuckoo's Nest, a novel by Ken Kesey, takes place in a mental institution and explores themes of individuality, freedom, and rebellion against oppressive systems. The protagonist, Randle P. McMurphy, a charismatic convict, fakes insanity to serve his sentence in a psychiatric hospital instead of prison. He clashes with Nurse Ratched, the authoritarian head nurse, and inspires the other patients to assert their independence. The story, narrated by Chief Bromden, a silent observer and fellow patient, examines the dynamics of power and the human spirit's resilience. Published in 1962, the book was adapted into a 1975 film that won five Academy Awards. It remains a poignant critique of institutional control and a celebration of nonconformity.",
                            PublishedDate = new DateTime(1962, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 6,
                            ShortDescription = "A story about individuality and institutional control.",
                            Title = "One Flew Over the Cuckoo's Nest"
                        },
                        new
                        {
                            Id = 6,
                            AuthorId = 1,
                            AverageRating = 4.8300000000000001,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://m.media-amazon.com/images/I/91U7HNa2NQL._AC_UF1000,1000_QL80_.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "The Shining, written by Stephen King, is a psychological horror novel set in the remote Overlook Hotel. Jack Torrance, an aspiring writer and recovering alcoholic, takes a job as the hotel's winter caretaker, bringing his wife Wendy and young son Danny with him. Danny possesses 'the shining,' a psychic ability that allows him to see the hotel's horrific past. As winter sets in, the isolation and supernatural forces within the hotel drive Jack into a murderous frenzy, threatening his family. Published in 1977, The Shining explores themes of addiction, domestic violence, and the fragility of sanity. The novel was adapted into a 1980 film by Stanley Kubrick, though it diverged from King's vision. A sequel, Doctor Sleep, was published in 2013, continuing Danny's story.",
                            PublishedDate = new DateTime(1977, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 6,
                            ShortDescription = "A chilling tale of isolation and madness.",
                            Title = "The Shining"
                        },
                        new
                        {
                            Id = 7,
                            AuthorId = 6,
                            AverageRating = 4.5999999999999996,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://m.media-amazon.com/images/I/91wOUFZCE+L._UF1000,1000_QL80_.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Dracula, written by Bram Stoker, is a classic Gothic horror novel that tells the story of Count Dracula's attempt to move from Transylvania to England in order to spread the undead curse, and his subsequent battle with a group of people led by the determined Professor Abraham Van Helsing. The novel is written in epistolary form, with the story told through letters, diary entries, newspaper clippings, and a ship's log. At the heart of Dracula is the struggle between good and evil, as the characters fight to destroy the vampire lord who threatens to spread his dark influence. Themes of fear, desire, sexuality, and superstition permeate the novel, along with reflections on Victorian society's attitudes toward these topics. Published in 1897, Dracula has had a profound impact on the vampire genre, inspiring numerous adaptations in film, television, and popular culture. The novel explores the dangers of unchecked power, the mystery of the unknown, and the terror of the supernatural, cementing Count Dracula as one of literature's most famous and enduring villains.",
                            PublishedDate = new DateTime(1897, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 6,
                            ShortDescription = "A gothic horror novel about the legendary vampire Count Dracula.",
                            Title = "Dracula"
                        },
                        new
                        {
                            Id = 8,
                            AuthorId = 1,
                            AverageRating = 5.0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1334416842i/830502.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Published in 1986, It is one of Stephen King's most iconic novels, a gripping horror story about the terror that haunts the small town of Derry, Maine. The novel follows a group of childhood friends who come together as adults to confront an ancient evil that takes the form of Pennywise, a shape-shifting entity that primarily appears as a killer clown. The novel explores the power of fear, the strength of friendship, and the eternal battle between good and evil. As the friends, who call themselves 'The Losers,' face off against Pennywise, they must confront their own childhood traumas and deepest fears. It is a chilling exploration of memory, courage, and the horrors of both childhood and adulthood. With its mix of supernatural terror and profound human emotion, It has become a cultural touchstone, inspiring a miniseries, films, and countless discussions about its themes of fear, friendship, and the horrors that lie beneath the surface of everyday life. King’s masterful storytelling and vivid portrayal of childhood and fear make It one of the most enduring works of horror fiction in the genre.",
                            PublishedDate = new DateTime(1986, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "A terrifying tale of childhood fears, and the evil that lurks beneath the surface of a small town.",
                            Title = "It"
                        },
                        new
                        {
                            Id = 9,
                            AuthorId = 5,
                            AverageRating = 5.0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://www.bookstation.ie/wp-content/uploads/2019/05/9780141036137.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "First published in 1945, Animal Farm by George Orwell is a powerful political allegory that critiques totalitarian regimes and explores the dangers of power and corruption. The novella is set on a farm where the animals overthrow their human oppressors and establish a new government, only to find that the new leadership, led by the pigs, becomes just as corrupt as the humans they replaced. The story, told through the animals' perspective, mirrors the events leading up to the Russian Revolution and the rise of Stalinism. Orwell's use of anthropomorphic animals to represent different political figures and ideologies makes Animal Farm both accessible and deeply poignant. Its themes of betrayal, exploitation, and the corruption of ideals are timeless, making Animal Farm a critical commentary on power, leadership, and the manipulation of the masses. The novella has had a significant impact on literature and political thought, remaining a key work in the discussion of totalitarianism and social justice.",
                            PublishedDate = new DateTime(1945, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 2,
                            ShortDescription = "A satirical allegory of totalitarianism, exploring the rise of power and corruption.",
                            Title = "Animal Farm"
                        },
                        new
                        {
                            Id = 10,
                            AuthorId = 7,
                            AverageRating = 4.2999999999999998,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://m.media-amazon.com/images/I/61rZCYUYXuL._AC_UF1000,1000_QL80_.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "First published in 1962, A Clockwork Orange by Anthony Burgess is a controversial and thought-provoking dystopian novel that examines the nature of free will, violence, and the conflict between individuality and societal control. The story is set in a near-future society and follows Alex, a teenage delinquent who leads a gang of criminals. Alex's journey is told in a unique style, using a fictional slang called 'Nadsat.' The novel explores the psychological and moral implications of the state-sponsored efforts to 'reform' Alex, subjecting him to a form of aversion therapy that strips him of his ability to choose between good and evil. Through this exploration, Burgess raises important questions about free will, the ethics of punishment, and the role of the state in shaping individual behavior. A Clockwork Orange is both a disturbing and insightful critique of modern society and its institutions. The novel has become a cultural touchstone, adapted into a film by Stanley Kubrick and continuing to inspire discussions on the nature of freedom, control, and human nature.",
                            PublishedDate = new DateTime(1962, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 6,
                            ShortDescription = "A dystopian novel that explores free will, violence, and the consequences of societal control.",
                            Title = "A Clockwork Orange"
                        },
                        new
                        {
                            Id = 11,
                            AuthorId = 8,
                            AverageRating = 4.25,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://lyceumtheatre.org/wp-content/uploads/2019/09/Murder-on-the-Orient-Express-WebPstr.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Murder on the Orient Express, first published in 1934, is one of Agatha Christie’s most famous works, featuring her legendary Belgian detective Hercule Poirot. The story takes place aboard the luxurious train, the Orient Express, where a wealthy American passenger, Samuel Ratchett, is found murdered in his compartment. Poirot, who happens to be traveling on the train, is asked to investigate the crime. As he delves deeper into the case, Poirot uncovers a complex web of lies and hidden motives. The passengers, all seemingly innocent, each have something to hide, and the detective must use his sharp mind to piece together the truth. Christie’s masterful plot, full of twists and red herrings, keeps readers guessing until the very end. The novel explores themes of justice, revenge, and the moral ambiguity of crime, making it a timeless and captivating mystery that has been adapted into numerous films and television series.",
                            PublishedDate = new DateTime(1934, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "A classic Hercule Poirot mystery aboard the luxurious Orient Express, filled with twists and a brilliant solution to a baffling crime.",
                            Title = "Murder on the Orient Express"
                        },
                        new
                        {
                            Id = 12,
                            AuthorId = 8,
                            AverageRating = 4.7000000000000002,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/5/57/The_Murder_of_Roger_Ackroyd_First_Edition_Cover_1926.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "The Murder of Roger Ackroyd, first published in 1926, is one of Agatha Christie's most groundbreaking works, featuring her famous detective Hercule Poirot. The story is set in the quiet village of King's Abbot, where the wealthy Roger Ackroyd is found murdered in his study. The case takes on new complexity when Poirot, who is retired in the village, is drawn into the investigation by Ackroyd's fiancée, Mrs. Ferrars, who has died under mysterious circumstances just days before. As Poirot begins to unravel the case, he discovers that nearly everyone in the village is hiding something, and he must use his unparalleled skills of deduction to piece together the truth. Christie’s brilliant twist ending revolutionized the genre and is still one of the most celebrated and discussed endings in detective fiction. The novel touches on themes of deceit, betrayal, and the nature of truth, making it a timeless classic in the mystery genre.",
                            PublishedDate = new DateTime(1926, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 3,
                            ShortDescription = "A groundbreaking Hercule Poirot mystery that reshaped the detective genre with its iconic twist ending.",
                            Title = "The Murder of Roger Ackroyd"
                        },
                        new
                        {
                            Id = 13,
                            AuthorId = 9,
                            AverageRating = 4.5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://m.media-amazon.com/images/I/91gmBp2wQNL._AC_UF894,1000_QL80_.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "East of Eden, published in 1952, is one of John Steinbeck’s most ambitious and famous works. The novel explores the complex relationships between two families, the Trasks and the Hamiltons, in California's Salinas Valley during the early 20th century. Central to the narrative are the themes of good versus evil, inherited sin, and the choices that define our lives. Steinbeck uses the biblical story of Cain and Abel as a backdrop, drawing parallels between the characters' struggles and moral dilemmas. The novel is a sweeping exploration of human nature, as well as the destructive impact of jealousy, desire, and guilt. With its vivid descriptions of the California landscape and rich characterizations, East of Eden is considered one of Steinbeck's greatest works, illustrating his deep understanding of the human condition.",
                            PublishedDate = new DateTime(1952, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "A sprawling, multigenerational story of good and evil, focusing on two families in California's Salinas Valley.",
                            Title = "East of Eden"
                        },
                        new
                        {
                            Id = 14,
                            AuthorId = 10,
                            AverageRating = 4.6699999999999999,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://www.book.store.bg/dcrimg/340714/dune.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "Dune, first published in 1965, is Frank Herbert's most iconic and influential novel, often regarded as one of the greatest works of science fiction. The novel is set on the desert planet of Arrakis, also known as Dune, the only source of the most valuable substance in the universe, spice melange. The story follows Paul Atreides, the young heir to House Atreides, as he navigates political intrigue, power struggles, and the harsh desert environment. The novel touches on themes of power, ecology, religion, and the future of humanity. Herbert creates a detailed world filled with complex social, political, and ecological systems that interweave throughout the narrative. *Dune* is renowned for its intricate plotting, philosophical depth, and exploration of human potential. Its impact on both science fiction and modern literature is immeasurable, and it remains a defining work of the genre.",
                            PublishedDate = new DateTime(1965, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 3,
                            ShortDescription = "A sweeping science fiction epic set on the desert planet Arrakis, exploring politics, religion, and ecology.",
                            Title = "Dune"
                        },
                        new
                        {
                            Id = 15,
                            AuthorId = 11,
                            AverageRating = 4.5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1404613595i/13.jpg",
                            IsApproved = true,
                            IsDeleted = false,
                            LongDescription = "The Hitchhiker's Guide to the Galaxy, first published in 1979, is Douglas Adams' most famous and influential work, blending science fiction, humor, and satire. The story begins with Arthur Dent, an ordinary man, who is suddenly whisked away from Earth just before it is destroyed to make way for an intergalactic freeway. Arthur joins Ford Prefect, an alien researcher for the titular Guide, on a wild journey through space, encountering strange planets, peculiar beings, and the galaxy's most incompetent bureaucracy. The novel explores themes of the absurdity of life, the meaning of existence, and the randomness of the universe, all wrapped in Adams' signature wit and absurdity. Known for its irreverence and humor, *The Hitchhiker's Guide to the Galaxy* has become a cult classic, inspiring numerous adaptations in radio, television, and film. Its influence on science fiction and comedy continues to resonate with readers and fans worldwide.",
                            PublishedDate = new DateTime(1979, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RatingsCount = 4,
                            ShortDescription = "A comedic science fiction adventure that follows Arthur Dent's absurd journey through space after Earth is destroyed.",
                            Title = "The Hitchhiker's Guide to the Galaxy"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Chat.Data.Models.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("BookHub.Server.Features.Chat.Data.Models.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("SenderId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("BookHub.Server.Features.Genre.Data.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Horror fiction is designed to scare, unsettle, or horrify readers. It explores themes of fear and the unknown, often incorporating supernatural elements like ghosts, monsters, or curses. The genre can also delve into the darker aspects of human psychology, portraying paranoia, obsession, and moral corruption. Subgenres include Gothic horror, psychological horror, and splatterpunk, each offering unique ways to evoke dread. Settings often amplify the tension, ranging from haunted houses to desolate landscapes, while the stories frequently address societal fears and existential questions.",
                            ImageUrl = "https://org-dcmp-staticassets.s3.us-east-1.amazonaws.com/posterimages/13453_1.jpg",
                            IsDeleted = false,
                            Name = "Horror"
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Science fiction explores futuristic, scientific, and technological themes, challenging readers to consider the possibilities and consequences of innovation. These stories often involve space exploration, artificial intelligence, time travel, or parallel universes. Beyond the speculative elements, science fiction frequently tackles ethical dilemmas, societal transformations, and the human condition. Subgenres include cyberpunk, space opera, and hard science fiction, each offering distinct visions of the future. The genre invites readers to imagine the impact of progress and to ponder humanity’s place in the cosmos.",
                            ImageUrl = "https://www.editoreric.com/greatlit/litgraphics/book-spiral-galaxy.jpg",
                            IsDeleted = false,
                            Name = "Science Fiction"
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Fantasy stories transport readers to magical realms filled with mythical creatures, enchanted objects, and epic quests. These tales often feature battles between good and evil, drawing upon folklore, mythology, and the human imagination. Characters may wield powerful magic or undertake journeys of self-discovery in richly crafted worlds. Subgenres like high fantasy, urban fantasy, and dark fantasy provide diverse settings and tones, appealing to a wide range of readers. Themes of heroism, destiny, and transformation are central to the genre, offering both escape and inspiration.",
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5EcrB6fhai5L3-7Ted6fZgxUjCti0W4avrA&s",
                            IsDeleted = false,
                            Name = "Fantasy"
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Mystery fiction is a puzzle-driven genre that engages readers with suspense and intrigue. The narrative typically revolves around solving a crime, uncovering hidden truths, or exposing a web of deceit. Protagonists range from amateur sleuths to seasoned detectives, each navigating clues, red herrings, and unexpected twists. Subgenres such as noir, cozy mysteries, and legal thrillers cater to varied tastes. Mystery stories often delve into human motives and societal dynamics, providing a satisfying journey toward uncovering the truth.",
                            ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/what-is-a-mystery.jpg",
                            IsDeleted = false,
                            Name = "Mystery"
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Romance novels celebrate the complexities of love and relationships, weaving stories of passion, connection, and emotional growth. They can be set in diverse contexts, from historical periods to fantastical worlds, and often feature characters overcoming personal or external obstacles to find happiness. Subgenres like contemporary romance, historical romance, and paranormal romance offer unique flavors and settings. The genre emphasizes emotional resonance, with narratives that inspire hope and affirm the power of love.",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/36/Hammond-SS10.jpg",
                            IsDeleted = false,
                            Name = "Romance"
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Thrillers are characterized by their fast-paced, high-stakes plots designed to keep readers on edge. They often involve life-and-death scenarios, sinister conspiracies, or relentless antagonists. The genre thrives on tension and unexpected twists, with protagonists racing against time to prevent disaster. Subgenres like psychological thrillers, spy thrillers, and action thrillers cater to diverse interests. The stories explore themes of survival, justice, and moral ambiguity, delivering an adrenaline-fueled reading experience.",
                            ImageUrl = "https://celadonbooks.com/wp-content/uploads/2019/10/what-is-a-thriller-1024x768.jpg",
                            IsDeleted = false,
                            Name = "Thriller"
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Adventure stories are dynamic tales of action, exploration, and survival. Protagonists often face daunting challenges, traversing uncharted territories or overcoming perilous odds. The genre celebrates courage, resilience, and the human spirit, taking readers on exhilarating journeys. From treasure hunts to epic battles, adventure fiction encompasses diverse settings and narratives. It appeals to those who crave excitement and the thrill of discovery.",
                            ImageUrl = "https://thumbs.dreamstime.com/b/open-book-ship-sailing-waves-concept-reading-adventure-literature-generative-ai-270347849.jpg",
                            IsDeleted = false,
                            Name = "Adventure"
                        },
                        new
                        {
                            Id = 8,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Historical fiction immerses readers in the past, blending factual events with fictional narratives to create vivid portrayals of bygone eras. These stories illuminate the lives, struggles, and triumphs of people from different times, providing insight into cultural, social, and political contexts. Subgenres include historical romance, historical mysteries, and alternate histories, each offering unique perspectives. The genre enriches our understanding of history while engaging us with compelling characters and plots.",
                            ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/Historical-Fiction-scaled.jpg",
                            IsDeleted = false,
                            Name = "Historical fiction"
                        },
                        new
                        {
                            Id = 9,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Biographies chronicle the lives of real individuals, offering intimate portraits of their experiences, achievements, and legacies. These works range from comprehensive life stories to focused accounts of specific events or periods. Biographies can inspire, inform, and provide deep insight into historical or contemporary figures. Autobiographies and memoirs, subgenres of biography, allow subjects to share their own narratives, adding personal depth to the genre.",
                            ImageUrl = "https://i0.wp.com/uspeakgreek.com/wp-content/uploads/2024/01/biography.webp?fit=780%2C780&ssl=1",
                            IsDeleted = false,
                            Name = "Biography"
                        },
                        new
                        {
                            Id = 10,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Self-help books are guides to personal growth, offering practical advice for improving one’s life. Topics range from mental health and relationships to productivity and spiritual fulfillment. The genre emphasizes empowerment, providing readers with strategies and tools for achieving goals and overcoming challenges. Subgenres include motivational literature, mindfulness guides, and career development books, catering to diverse needs and aspirations.",
                            ImageUrl = "https://www.wellnessroadpsychology.com/wp-content/uploads/2024/05/Self-Help.jpg",
                            IsDeleted = false,
                            Name = "Self-help"
                        },
                        new
                        {
                            Id = 11,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Non-fiction encompasses works rooted in factual information, offering insights into real-world topics. It spans memoirs, investigative journalism, essays, and academic studies, covering subjects like history, science, culture, and politics. The genre educates and engages readers, often challenging perceptions and broadening understanding. Non-fiction can be narrative-driven or expository, appealing to those seeking knowledge or a deeper connection to reality.",
                            ImageUrl = "https://pickbestbook.com/wp-content/uploads/2023/06/Nonfiction-Literature-1.png",
                            IsDeleted = false,
                            Name = "Non-fiction"
                        },
                        new
                        {
                            Id = 12,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Poetry is a literary form that condenses emotions, thoughts, and imagery into carefully chosen words, often structured with rhythm and meter. It explores universal themes such as love, nature, grief, and introspection, offering readers profound and evocative experiences. From traditional sonnets and haikus to free verse and spoken word, poetry captivates through its ability to articulate the inexpressible, creating deep emotional resonance and intellectual reflection.",
                            ImageUrl = "https://assets.ltkcontent.com/images/9037/examples-of-poetry-genres_7abbbb2796.jpg",
                            IsDeleted = false,
                            Name = "Poetry"
                        },
                        new
                        {
                            Id = 13,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Drama fiction delves into emotional and relational conflicts, portraying the complexities of human interactions and emotions. It emphasizes character development and nuanced storytelling, often exploring themes of love, betrayal, identity, and societal struggles. Drama offers readers a lens into the intricacies of the human experience, whether through tragic, romantic, or morally ambiguous narratives. Its focus on realism and emotional depth creates stories that resonate deeply with audiences.",
                            ImageUrl = "https://basudewacademichub.in/wp-content/uploads/2024/02/drama-literature-solution.png",
                            IsDeleted = false,
                            Name = "Drama"
                        },
                        new
                        {
                            Id = 14,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Children's literature is crafted to captivate and inspire young readers with imaginative worlds, moral lessons, and relatable characters. These stories often emphasize themes of curiosity, friendship, and bravery, delivering messages of kindness, resilience, and growth. From whimsical picture books to adventurous chapter books, children's fiction nurtures creativity and fosters a lifelong love of reading, helping young minds explore both real and fantastical realms.",
                            ImageUrl = "https://media.vanityfair.com/photos/598888671dc63c45b7b1db6e/master/w_2560%2Cc_limit/MAG-0817-Wild-Things-a.jpg",
                            IsDeleted = false,
                            Name = "Children's"
                        },
                        new
                        {
                            Id = 15,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Young Adult (YA) fiction speaks to the unique experiences and challenges of adolescence, addressing themes such as identity, first love, friendship, and coming of age. These stories often feature relatable protagonists navigating personal growth, societal expectations, and emotional upheaval. Subgenres such as fantasy, dystopian, and contemporary YA provide diverse backdrops for these journeys, resonating with readers through authentic and engaging storytelling that reflects their own struggles and triumphs.",
                            ImageUrl = "https://m.media-amazon.com/images/I/81xRLF1KCAL._AC_UF1000,1000_QL80_.jpg",
                            IsDeleted = false,
                            Name = "Young Adult"
                        },
                        new
                        {
                            Id = 16,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Comedy fiction aims to entertain and delight readers through humor, satire, and absurdity. It uses wit and clever storytelling to highlight human follies, societal quirks, or surreal situations. From lighthearted escapades to biting social commentary, comedy encompasses a range of tones and styles. The genre often brings laughter and joy, offering an escape from the mundane while sometimes delivering thought-provoking messages in the guise of humor.",
                            ImageUrl = "https://mandyevebarnett.com/wp-content/uploads/2017/12/humor.jpg?w=640",
                            IsDeleted = false,
                            Name = "Comedy"
                        },
                        new
                        {
                            Id = 17,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Graphic novels seamlessly blend visual art and narrative storytelling, using a combination of text and illustrations to convey complex plots and emotions. This versatile format spans a wide array of genres, including superhero tales, memoirs, historical epics, and science fiction. Graphic novels offer an immersive reading experience, appealing to diverse audiences through their ability to convey vivid imagery and intricate storylines that are as impactful as traditional prose.",
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSb0THovTlPB_nRl3RY6TsbWD4R2qEC-TQSAg&s",
                            IsDeleted = false,
                            Name = "Graphic Novel"
                        },
                        new
                        {
                            Id = 18,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "The 'Other' genre serves as a home for unconventional, experimental, or cross-genre works that defy traditional categorization. This category embraces innovation and diversity, welcoming stories that push the boundaries of storytelling, structure, and style. From hybrid narratives to avant-garde experiments, 'Other' offers a platform for unique voices and creative expressions that don’t fit neatly into predefined genres.",
                            ImageUrl = "https://www.98thpercentile.com/hubfs/388x203%20(4).png",
                            IsDeleted = false,
                            Name = "Other"
                        },
                        new
                        {
                            Id = 19,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Dystopian fiction paints a grim portrait of societies marred by oppression, inequality, or disaster, often set in a future shaped by catastrophic events or authoritarian regimes. These cautionary tales explore themes like survival, rebellion, and the loss of humanity, serving as critiques of political, social, and environmental trends. Subgenres such as post-apocalyptic and cyber-dystopia examine the fragility of civilization and the consequences of unchecked power or technological overreach.",
                            ImageUrl = "https://www.ideology-theory-practice.org/uploads/1/3/5/5/135563566/050_orig.jpg",
                            IsDeleted = false,
                            Name = "Dystopian"
                        },
                        new
                        {
                            Id = 20,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Spirituality books delve into the deeper questions of existence, faith, and the human soul, offering insights and practices to nurture inner peace and personal growth. They often explore themes of mindfulness, self-awareness, and connection to a higher power or universal energy. From philosophical reflections to practical guides, these works resonate with readers seekinginspiration, understanding, and spiritual fulfillment across diverse traditions and belief systems.",
                            ImageUrl = "https://m.media-amazon.com/images/I/61jxcM3UskL._AC_UF1000,1000_QL80_.jpg",
                            IsDeleted = false,
                            Name = "Spirituality"
                        },
                        new
                        {
                            Id = 21,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Crime fiction is centered around the investigation of a crime, often focusing on the detection of criminals or the pursuit of justice. It may include detectives, police officers, or amateur sleuths solving crimes like murder, theft, or corruption. The genre can involve suspense, action, and exploration of moral dilemmas surrounding law and order. Subgenres include hardboiledcrime, cozy mysteries, and police procedurals, all providing different approaches to solving crimes and investigating human behavior.",
                            ImageUrl = "https://img.tpt.cloud/nextavenue/uploads/2019/04/Crime-Fiction-Savvy-Sleuths-Over-50_53473532.inside.1200x775.jpg",
                            IsDeleted = false,
                            Name = "Crime"
                        },
                        new
                        {
                            Id = 22,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Urban fiction explores life in modern, often gritty urban settings, focusing on the struggles, relationships, and experiences of people in cities. This genre frequently addresses themes like poverty, crime, social injustice, and community dynamics. It can incorporate elements of drama, romance, and even horror, often portraying the challenges of urban life with raw, unflinching realism. Urban fiction is popular in contemporary literature and often includes characters from marginalized communities.",
                            ImageUrl = "https://frugalbookstore.net/cdn/shop/collections/Urban-Fiction.png?v=1724599745&width=480",
                            IsDeleted = false,
                            Name = "Urban Fiction"
                        },
                        new
                        {
                            Id = 23,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Fairy tale fiction involves magical or fantastical stories often set in a world where magic and mythical creatures exist. These stories typically follow a clear moral arc, with characters who experience trials or transformation before achieving a happy ending. Fairy tales often feature archetypal characters like witches, princes, and princesses, and they explore themes of good vs. evil, justice, and personal growth. Many fairy tales have been passed down through generations, and the genre continues to inspire modern adaptations and retellings.",
                            ImageUrl = "https://news.syr.edu/wp-content/uploads/2023/09/enchanting_fairy_tale_woodland_onto_a_castle_an.original-scaled.jpg",
                            IsDeleted = false,
                            Name = "Fairy Tale"
                        },
                        new
                        {
                            Id = 24,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Epic fiction is characterized by large-scale, grand narratives often centered around heroic characters or monumental events. Epics typically focus on the struggles and triumphs of protagonists who undergo significant personal or societal change. These stories often span extensive periods of time and encompass entire civilizations, exploring themes like war, leadership, and cultural identity. Classic examples include *The Iliad* and *The Odyssey*, with modern epics continuing to explore the human experience in vast, sweeping terms.",
                            ImageUrl = "https://i0.wp.com/joncronshaw.com/wp-content/uploads/2024/01/DALL%C2%B7E-2024-01-17-09.05.10-A-magical-and-enchanting-landscape-for-a-fantasy-blog-post-featuring-an-ancient-castle-perched-on-a-high-cliff-a-vast-mystical-forest-with-towering.png?fit=1200%2C686&ssl=1",
                            IsDeleted = false,
                            Name = "Epic"
                        },
                        new
                        {
                            Id = 25,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Political fiction uses stories to explore, criticize, or comment on political systems, ideologies, and power dynamics. These narratives often examine how political structures affect individuals and societies, focusing on themes of corruption, revolution, and social change. Political fiction can include dystopian novels, satires, and thrillers, offering commentary on both contemporary and historical politics. Through these stories, authors challenge readers to think critically about the systems that govern their lives.",
                            ImageUrl = "https://markelayat.com/wp-content/uploads/elementor/thumbs/Political-Fiction-ft-image-qwo9yzatn5xk8t34vvqfivz2ed7zuj5lccn9ylm7bc.png",
                            IsDeleted = false,
                            Name = "Political Fiction"
                        },
                        new
                        {
                            Id = 26,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Philosophical fiction delves into profound questions about existence, ethics, free will, and the nature of reality. These novels often explore abstract ideas and are driven by deep intellectual themes rather than plot or action. Philosophical fiction may follow characters who engage in critical thinking, self-reflection, or existential crises. These works often question the meaning of life, morality, and consciousness, and they can be a blend of both fiction and philosophy, prompting readers to consider their own beliefs and perspectives.",
                            ImageUrl = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1546103428i/5297._UX160_.jpg",
                            IsDeleted = false,
                            Name = "Philosophical Fiction"
                        },
                        new
                        {
                            Id = 27,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "True crime fiction is based on real-life criminal events, recounting the details of notorious crimes, investigations, and trials. It often focuses on infamous cases, delving into the psychology of criminals, the detectives or journalists who solve the cases, and the social impact of the crime. True crime often incorporates extensive research and interviews, giving readers an inside look at the complexities of real-life crime and law enforcement. These works can be chilling and thought-provoking, blending elements of mystery, drama,and historical non-fiction.",
                            ImageUrl = "https://is1-ssl.mzstatic.com/image/thumb/Podcasts221/v4/00/07/67/000767b5-bad1-5d78-db34-373363ec6b3e/mza_8962416523973028402.jpg/1200x1200bf.webp",
                            IsDeleted = false,
                            Name = "True Crime"
                        },
                        new
                        {
                            Id = 28,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Satire is a genre that uses humor, irony, and exaggeration to criticize or mock individuals, institutions, or societal norms. It often employs wit and sarcasm to highlight the flaws and absurdities of the subject being criticized, sometimes with the intent of provoking thought or promoting change. Satirical works can cover a wide range of topics, including politics, culture, and human nature, and can be both lighthearted or dark in tone. Famous examples include works like Gulliver's Travels and Catch-22.",
                            ImageUrl = "https://photos.demandstudios.com/getty/article/64/32/529801877.jpg",
                            IsDeleted = false,
                            Name = "Satire"
                        },
                        new
                        {
                            Id = 29,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Psychological fiction delves into the inner workings of the mind, exploring complex emotional states, mental illness, and the psychological effects of personal trauma, relationships, and societal pressures. These works often focus on character development and the emotional or mental struggles of the protagonists, rather than external events. Psychological fiction can blur the lines between reality and illusion, questioning perceptions and exploring the deeper layers of human consciousness. It often presents challenging and sometimes disturbing narratives about identity and self-perception. Notable examples include The Bell Jar and The Catcher in the Rye.",
                            ImageUrl = "https://literaturelegends.com/wp-content/uploads/2023/08/psychological.jpg",
                            IsDeleted = false,
                            Name = "Psychological Fiction"
                        },
                        new
                        {
                            Id = 30,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Supernatural fiction explores phenomena beyond the natural world, often incorporating ghosts, spirits, vampires, or otherworldly beings. These works blend elements of horror, fantasy, and the unexplained, and often delve into themes of life after death, paranormal activity, and other mystifying occurrences. The supernatural genre captivates readers with its portrayal of eerie events and the unknown, often blurring the line between reality and the mystical. Examples include works like The Haunting of Hill House and The Turn of the Screw.",
                            ImageUrl = "https://fully-booked.ca/wp-content/uploads/2024/02/evolution-of-paranormal-fiction-1024x576.jpg",
                            IsDeleted = false,
                            Name = "Supernatural"
                        },
                        new
                        {
                            Id = 31,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Gothic fiction is characterized by its dark, eerie atmosphere, and often involves elements of horror, mystery, and the supernatural. These stories typically feature gloomy, decaying settings such as castles, mansions, or haunted landscapes, and often include tragic or macabre themes. Gothic fiction focuses on emotions like fear, dread, and despair, and explores the darker sides of human nature. Famous examples include works like Wuthering Heights and Frankenstein.",
                            ImageUrl = "https://bookstr.com/wp-content/uploads/2022/09/V8mj92.webp",
                            IsDeleted = false,
                            Name = "Gothic Fiction"
                        },
                        new
                        {
                            Id = 32,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Magical realism blends elements of magic or the supernatural with a realistic narrative, creating a world where extraordinary events occur within ordinary settings. This genre often explores themes of identity, culture, and human experience, and it is marked by the seamless integration of magical elements into everyday life. Prominent examples include books like One Hundred Years of Solitude and The House of the Spirits.",
                            ImageUrl = "https://www.world-defined.com/wp-content/uploads/2024/04/Magic-Realism-Books-978x652-1.webp",
                            IsDeleted = false,
                            Name = "Magical Realism"
                        },
                        new
                        {
                            Id = 33,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Dark fantasy combines elements of fantasy with a sense of horror, despair, and the supernatural. These stories often take place in dark, gritty worlds where magic, danger, and moral ambiguity challenge the characters. Dark fantasy blends the fantastical with the disturbing, creating a sense of dread and unease. Examples include books like The Dark Tower series and A Song of Ice and Fire.",
                            ImageUrl = "https://miro.medium.com/v2/resize:fit:1024/1*VU5O34UlH-1SXZkEnL0dyg.jpeg",
                            IsDeleted = false,
                            Name = "Dark Fantasy"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Identity.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "user1Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c7c583f2-0dfe-49c6-8943-f9e90e356d3f",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user1@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "f95ce8bd-ca5b-4b3c-b5f3-00e07635e670",
                            TwoFactorEnabled = false,
                            UserName = "user1name"
                        },
                        new
                        {
                            Id = "user2Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "5c643883-1e0c-46ff-b74c-559dd09b01dc",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user2@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "8e91d02f-6a4c-41f8-bf00-7d279637f806",
                            TwoFactorEnabled = false,
                            UserName = "user2name"
                        },
                        new
                        {
                            Id = "user3Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "4f7f34ec-aa9a-46e4-b949-5fe4f8b10d8c",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user3@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "8d154e3e-33e9-4d0d-9ede-491a74bf207e",
                            TwoFactorEnabled = false,
                            UserName = "user3name"
                        },
                        new
                        {
                            Id = "user4Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "ffad8950-6969-4861-bb4d-0dfe6a07ebd4",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user4@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "9b24f9d4-c0d3-4105-b6b1-366f4f225633",
                            TwoFactorEnabled = false,
                            UserName = "user4name"
                        },
                        new
                        {
                            Id = "user5Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e990032e-4bd0-4031-917a-6e1d45ff9718",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user5@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "86625e2c-ba27-4ed5-9cfb-8d0ee646e77a",
                            TwoFactorEnabled = false,
                            UserName = "user5name"
                        },
                        new
                        {
                            Id = "user6Id",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "111c598a-425b-42e7-8350-05765215e6cd",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user6@mail.com",
                            EmailConfirmed = false,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "3eec17d3-fce7-469d-883e-66c947b798e7",
                            TwoFactorEnabled = false,
                            UserName = "user6name"
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Notification.Data.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResourceId")
                        .HasColumnType("int");

                    b.Property<string>("ResourceType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BookHub.Server.Features.ReadingList.Data.Models.ReadingList", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "BookId", "Status");

                    b.HasIndex("BookId");

                    b.ToTable("ReadingLists");

                    b.HasData(
                        new
                        {
                            UserId = "user1Id",
                            BookId = 1,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user1Id",
                            BookId = 2,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user1Id",
                            BookId = 3,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user1Id",
                            BookId = 4,
                            Status = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user1Id",
                            BookId = 5,
                            Status = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user1Id",
                            BookId = 6,
                            Status = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user2Id",
                            BookId = 1,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user2Id",
                            BookId = 5,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user2Id",
                            BookId = 6,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user2Id",
                            BookId = 10,
                            Status = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user2Id",
                            BookId = 12,
                            Status = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user3Id",
                            BookId = 10,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user3Id",
                            BookId = 7,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user3Id",
                            BookId = 8,
                            Status = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user3Id",
                            BookId = 4,
                            Status = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user3Id",
                            BookId = 12,
                            Status = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            UserId = "user3Id",
                            BookId = 13,
                            Status = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Review.Data.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Reviews");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookId = 1,
                            Content = "A truly chilling tale. King masterfully explores the dark side of human grief and love.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 2,
                            BookId = 1,
                            Content = "The book was gripping but felt a bit too disturbing at times for my taste.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 3
                        },
                        new
                        {
                            Id = 3,
                            BookId = 1,
                            Content = "An unforgettable story that haunts you long after you've finished it. Highly recommended!",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 4,
                            BookId = 1,
                            Content = "The characters were well-developed, but the plot felt predictable toward the end.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 5,
                            BookId = 2,
                            Content = "An incredible conclusion to the series. Every twist and turn kept me on edge.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 6,
                            BookId = 2,
                            Content = "The Battle of Hogwarts was epic! A bittersweet yet satisfying ending.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 7,
                            BookId = 2,
                            Content = "I expected more from some of the character arcs, but still a solid read.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 8,
                            BookId = 2,
                            Content = "Rowling’s world-building continues to amaze, even in the final installment.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 9,
                            BookId = 3,
                            Content = "A timeless masterpiece. Tolkien’s world and characters are unmatched in depth and richness.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 10,
                            BookId = 3,
                            Content = "The pacing was slow at times, but the payoff in the end was well worth it.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 11,
                            BookId = 3,
                            Content = "The bond between Sam and Frodo is the heart of this epic journey. Beautifully written.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 12,
                            BookId = 3,
                            Content = "An epic tale that defines the fantasy genre. Loved every moment of it.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 13,
                            BookId = 3,
                            Content = "The attention to detail in Middle-earth is staggering. Tolkien is a true genius.",
                            CreatedBy = "user5name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 14,
                            BookId = 3,
                            Content = "A bit long for my liking, but undeniably one of the greatest stories ever told.",
                            CreatedBy = "user6name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 15,
                            BookId = 4,
                            Content = "A haunting vision of the future that feels increasingly relevant today.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 16,
                            BookId = 4,
                            Content = "An intense and thought-provoking read. Orwell's insights are unparalleled.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 17,
                            BookId = 4,
                            Content = "I struggled with some parts, but the message is profoundly impactful.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 18,
                            BookId = 4,
                            Content = "A must-read for anyone concerned about privacy and freedom.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 19,
                            BookId = 4,
                            Content = "Chilling and unforgettable. Orwell's world feels disturbingly possible.",
                            CreatedBy = "user5name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 20,
                            BookId = 5,
                            Content = "McMurphy is such a memorable character. This book stays with you.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 21,
                            BookId = 5,
                            Content = "Kesey’s writing is poetic and raw. A heartbreaking tale of rebellion.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 22,
                            BookId = 5,
                            Content = "I loved how the narrative builds through Chief Bromden's perspective.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 23,
                            BookId = 5,
                            Content = "A sobering exploration of power and freedom within a broken system.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 24,
                            BookId = 5,
                            Content = "The tension between McMurphy and Nurse Ratched is electric.",
                            CreatedBy = "user5name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 25,
                            BookId = 5,
                            Content = "A deeply human story that makes you think about society’s flaws.",
                            CreatedBy = "user6name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 26,
                            BookId = 6,
                            Content = "Terrifying and beautifully written. Stephen King is at his best.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 27,
                            BookId = 6,
                            Content = "Danny’s ‘shining’ powers add so much depth to this chilling story.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 28,
                            BookId = 6,
                            Content = "The Overlook Hotel is as much a character as the Torrance family.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 29,
                            BookId = 6,
                            Content = "I couldn’t put it down! The suspense is incredible.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 30,
                            BookId = 6,
                            Content = "Jack’s descent into madness is both horrifying and tragic.",
                            CreatedBy = "user5name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 31,
                            BookId = 6,
                            Content = "King perfectly balances psychological and supernatural horror.",
                            CreatedBy = "user6name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 32,
                            BookId = 7,
                            Content = "A timeless classic that redefined the vampire genre. Dracula is both terrifying and captivating.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 33,
                            BookId = 7,
                            Content = "The atmosphere of dread and suspense is palpable. Stoker's writing is chilling and immersive.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 34,
                            BookId = 7,
                            Content = "Count Dracula is a villain for the ages. A fascinating tale of good versus evil with unforgettable characters.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 35,
                            BookId = 7,
                            Content = "A brilliant mix of Gothic horror and suspense. Stoker’s world-building and vivid imagery are unmatched.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 36,
                            BookId = 7,
                            Content = "A dark, thrilling journey that explores the depths of fear and obsession. Dracula remains one of the greatest horror novels ever written.",
                            CreatedBy = "user5name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 37,
                            BookId = 8,
                            Content = "A terrifying masterpiece that captures the horror of childhood fears and the bond of friendship. Pennywise is unforgettable.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 38,
                            BookId = 8,
                            Content = "Stephen King's storytelling is unparalleled. *It* is a terrifying and emotionally resonant journey that stays with you long after finishing.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 39,
                            BookId = 8,
                            Content = "The depth of character development and the chilling horror elements make *It* an instant classic. A must-read for any horror fan.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 40,
                            BookId = 8,
                            Content = "An epic tale of fear, courage, and friendship. King's ability to balance the supernatural with deeply human emotions is astounding.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 41,
                            BookId = 9,
                            Content = "A powerful allegory that exposes the dangers of totalitarianism. Orwell's sharp critique of power and corruption is as relevant today as ever.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 42,
                            BookId = 9,
                            Content = "A brilliantly crafted political satire that uses animals to dissect the flaws of human nature and governance. A must-read for anyone interested in societal dynamics.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 43,
                            BookId = 10,
                            Content = "A bold exploration of free will, violence, and societal control. Burgess' unique language and dark humor make this a thought-provoking read.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 44,
                            BookId = 10,
                            Content = "A chilling and unsettling look at the consequences of state control over individual freedom. The invented language makes it even more immersive.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 45,
                            BookId = 10,
                            Content = "This novel challenges ideas of morality and free will, with a disturbing yet captivating narrative. It's hard to put down once you start.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 46,
                            BookId = 10,
                            Content = "A powerful exploration of violence and society, but the language barrier can be a bit hard to get used to at first. Still, it's a masterpiece of dystopian fiction.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 47,
                            BookId = 10,
                            Content = "An unforgettable story that questions the nature of human behavior and control. The narrative is dark and surreal, but it raises important questions about freedom and choice.",
                            CreatedBy = "user5name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 48,
                            BookId = 10,
                            Content = "A fascinating, though disturbing, book that delves into the mind of its violent protagonist. Its style is unique, but not everyone will appreciate it.",
                            CreatedBy = "user6name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsDeleted = false,
                            Rating = 3
                        },
                        new
                        {
                            Id = 49,
                            BookId = 11,
                            Content = "A masterclass in mystery writing. Agatha Christie's ability to weave a complex plot with unexpected twists is unparalleled. A must-read for mystery lovers!",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 50,
                            BookId = 11,
                            Content = "A brilliant detective story filled with intrigue and suspense. The ending is truly unexpected, and the characters are well-developed. One of the best Poirot novels.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 51,
                            BookId = 11,
                            Content = "While the plot is engaging, I found some of the character motivations a bit too contrived. Still, it's an enjoyable read for fans of classic detective fiction.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 52,
                            BookId = 11,
                            Content = "This is a solid mystery, but the conclusion felt a bit rushed. Poirot's reasoning is impeccable, but I was hoping for a more satisfying resolution.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 3
                        },
                        new
                        {
                            Id = 53,
                            BookId = 12,
                            Content = "A brilliant and mind-bending mystery with one of the best plot twists in literary history. Agatha Christie at her finest!",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 54,
                            BookId = 12,
                            Content = "An incredibly well-crafted story. The twist was shocking, but the pacing felt slow at times. Still, an iconic piece of detective fiction.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 55,
                            BookId = 12,
                            Content = "Christie delivers a thrilling and suspenseful plot that kept me guessing until the very end. The twist was absolutely unexpected, making this a standout in the genre.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 56,
                            BookId = 13,
                            Content = "A masterwork of fiction that delves into the depths of human nature. Steinbeck's exploration of good and evil, coupled with his rich characters and vivid settings, makes this novel unforgettable.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 57,
                            BookId = 13,
                            Content = "East of Eden is an epic story, but it can feel overwhelming at times. While the themes of sin and redemption are powerful, the pacing can be slow in parts. However, Steinbeck's writing is undeniably brilliant.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 58,
                            BookId = 13,
                            Content = "An incredibly complex and thought-provoking novel. The story of the Trask and Hamilton families is both heartbreaking and inspiring. Steinbeck creates unforgettable characters and a gripping narrative.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 59,
                            BookId = 13,
                            Content = "While I found the writing to be beautiful, the novel's length and depth made it a challenging read. However, it’s clear why East of Eden is considered one of Steinbeck's masterpieces.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 60,
                            BookId = 14,
                            Content = "A groundbreaking masterpiece in the sci-fi genre. *Dune* is a deeply philosophical and complex novel that explores themes of politics, religion, and ecology. The world-building is extraordinary, and the story's depth will stay with you long after you've finished reading.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 61,
                            BookId = 14,
                            Content = "Herbert's *Dune* is an epic tale with intricate world-building and a complex, multi-layered narrative. While it can be slow-paced at times, the book offers a compelling story of power, survival, and human evolution. Definitely a must-read for science fiction enthusiasts.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 62,
                            BookId = 14,
                            Content = "A truly immersive experience that challenges readers with its philosophical insights and political commentary. *Dune* is a dense, but rewarding read that covers timeless themes of leadership, environmental stewardship, and human resilience. However, it can be heavy and difficult to follow at times.",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 63,
                            BookId = 15,
                            Content = "A brilliant mix of wit and sci-fi! Douglas Adams' humor shines through in every page, making this a memorable and laugh-out-loud read. A true classic of the genre.",
                            CreatedBy = "user1name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 64,
                            BookId = 15,
                            Content = "An absolutely hilarious and thought-provoking journey through space. The absurdity of the plot paired with sharp social commentary makes it both entertaining and intelligent.",
                            CreatedBy = "user2name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsDeleted = false,
                            Rating = 4
                        },
                        new
                        {
                            Id = 65,
                            BookId = 15,
                            Content = "The Hitchhiker's Guide to the Galaxy is a rollercoaster of absurdity, with quirky characters and laugh-out-loud moments. The satire on life, the universe, and everything is simply genius!",
                            CreatedBy = "user3name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsDeleted = false,
                            Rating = 5
                        },
                        new
                        {
                            Id = 66,
                            BookId = 15,
                            Content = "A fun and wildly entertaining read, though the randomness and eccentricities of the story may be off-putting for some. Still, it's a fantastic book that has earned its place in sci-fi history.",
                            CreatedBy = "user4name",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsDeleted = false,
                            Rating = 4
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.Review.Data.Models.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsUpvote")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReviewId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ReviewId");

                    b.ToTable("Votes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsUpvote = true,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsUpvote = false,
                            ReviewId = 1
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 2
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 2
                        },
                        new
                        {
                            Id = 8,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 15
                        },
                        new
                        {
                            Id = 9,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 15
                        },
                        new
                        {
                            Id = 10,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsUpvote = true,
                            ReviewId = 15
                        },
                        new
                        {
                            Id = 11,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 15
                        },
                        new
                        {
                            Id = 12,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 20
                        },
                        new
                        {
                            Id = 13,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 20
                        },
                        new
                        {
                            Id = 14,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 20
                        },
                        new
                        {
                            Id = 15,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 26
                        },
                        new
                        {
                            Id = 16,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 26
                        },
                        new
                        {
                            Id = 17,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsUpvote = true,
                            ReviewId = 26
                        },
                        new
                        {
                            Id = 18,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsUpvote = true,
                            ReviewId = 32
                        },
                        new
                        {
                            Id = 19,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 32
                        },
                        new
                        {
                            Id = 20,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 32
                        },
                        new
                        {
                            Id = 21,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user4Id",
                            IsUpvote = true,
                            ReviewId = 32
                        },
                        new
                        {
                            Id = 22,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user5Id",
                            IsUpvote = true,
                            ReviewId = 32
                        },
                        new
                        {
                            Id = 23,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user6Id",
                            IsUpvote = false,
                            ReviewId = 32
                        },
                        new
                        {
                            Id = 24,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsUpvote = true,
                            ReviewId = 37
                        },
                        new
                        {
                            Id = 25,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 37
                        },
                        new
                        {
                            Id = 26,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 37
                        },
                        new
                        {
                            Id = 27,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsUpvote = true,
                            ReviewId = 41
                        },
                        new
                        {
                            Id = 28,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 41
                        },
                        new
                        {
                            Id = 29,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 41
                        },
                        new
                        {
                            Id = 30,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user1Id",
                            IsUpvote = true,
                            ReviewId = 43
                        },
                        new
                        {
                            Id = 31,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 43
                        },
                        new
                        {
                            Id = 32,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user3Id",
                            IsUpvote = true,
                            ReviewId = 43
                        },
                        new
                        {
                            Id = 33,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 49
                        },
                        new
                        {
                            Id = 34,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 53
                        },
                        new
                        {
                            Id = 35,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 56
                        },
                        new
                        {
                            Id = 36,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 60
                        },
                        new
                        {
                            Id = 37,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatorId = "user2Id",
                            IsUpvote = true,
                            ReviewId = 63
                        });
                });

            modelBuilder.Entity("BookHub.Server.Features.UserProfile.Data.Models.UserProfile", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Biography")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("CreatedAuthorsCount")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBooksCount")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentlyReadingBooksCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("ReadBooksCount")
                        .HasColumnType("int");

                    b.Property<int>("ReviewsCount")
                        .HasColumnType("int");

                    b.Property<string>("SocialMediaUrl")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("ToReadBooksCount")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Profiles");

                    b.HasData(
                        new
                        {
                            UserId = "user1Id",
                            Biography = "John is a passionate reader and a book reviewer.",
                            CreatedAuthorsCount = 0,
                            CreatedBooksCount = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CurrentlyReadingBooksCount = 1,
                            DateOfBirth = new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "John",
                            ImageUrl = "https://www.shareicon.net/data/512x512/2016/05/24/770117_people_512x512.png",
                            IsPrivate = false,
                            LastName = "Doe",
                            PhoneNumber = "+1234567890",
                            ReadBooksCount = 3,
                            ReviewsCount = 15,
                            SocialMediaUrl = "https://twitter.com/johndoe",
                            ToReadBooksCount = 2
                        },
                        new
                        {
                            UserId = "user2Id",
                            Biography = "Alice enjoys exploring fantasy and sci-fi genres.",
                            CreatedAuthorsCount = 0,
                            CreatedBooksCount = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CurrentlyReadingBooksCount = 1,
                            DateOfBirth = new DateTime(1985, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Alice",
                            ImageUrl = "https://static.vecteezy.com/system/resources/previews/002/002/257/non_2x/beautiful-woman-avatar-character-icon-free-vector.jpg",
                            IsPrivate = false,
                            LastName = "Smith",
                            PhoneNumber = "+1987654321",
                            ReadBooksCount = 3,
                            ReviewsCount = 15,
                            SocialMediaUrl = "https://facebook.com/alicesmith",
                            ToReadBooksCount = 1
                        },
                        new
                        {
                            UserId = "user3Id",
                            Biography = "Bob is a new reader with a love for thrillers and mysteries.",
                            CreatedAuthorsCount = 0,
                            CreatedBooksCount = 0,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CurrentlyReadingBooksCount = 2,
                            DateOfBirth = new DateTime(2000, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Bob",
                            ImageUrl = "https://cdn1.iconfinder.com/data/icons/user-pictures/101/malecostume-512.png",
                            IsPrivate = false,
                            LastName = "Johnson",
                            PhoneNumber = "+1122334455",
                            ReadBooksCount = 3,
                            ReviewsCount = 14,
                            SocialMediaUrl = "https://instagram.com/bobjohnson",
                            ToReadBooksCount = 1
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Shared.BookGenre.BookGenre", b =>
                {
                    b.HasOne("BookHub.Server.Features.Book.Data.Models.Book", "Book")
                        .WithMany("BooksGenres")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Features.Genre.Data.Models.Genre", "Genre")
                        .WithMany("BooksGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("BookHub.Server.Data.Models.Shared.ChatUser.ChatUser", b =>
                {
                    b.HasOne("BookHub.Server.Features.Chat.Data.Models.Chat", "Chat")
                        .WithMany("ChatsUsers")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "User")
                        .WithMany("ChatsUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookHub.Server.Features.Authors.Data.Models.Author", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "Creator")
                        .WithMany("Authors")
                        .HasForeignKey("CreatorId");

                    b.HasOne("BookHub.Server.Features.Authors.Data.Models.Nationality", "Nationality")
                        .WithMany("Authors")
                        .HasForeignKey("NationalityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Nationality");
                });

            modelBuilder.Entity("BookHub.Server.Features.Book.Data.Models.Book", b =>
                {
                    b.HasOne("BookHub.Server.Features.Authors.Data.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId");

                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "Creator")
                        .WithMany("Books")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Author");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("BookHub.Server.Features.Chat.Data.Models.Chat", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "Creator")
                        .WithMany("ChatsCreated")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("BookHub.Server.Features.Chat.Data.Models.ChatMessage", b =>
                {
                    b.HasOne("BookHub.Server.Features.Chat.Data.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "Sender")
                        .WithMany("SentChatMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("BookHub.Server.Features.Notification.Data.Models.Notification", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookHub.Server.Features.ReadingList.Data.Models.ReadingList", b =>
                {
                    b.HasOne("BookHub.Server.Features.Book.Data.Models.Book", "Book")
                        .WithMany("ReadingLists")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "User")
                        .WithMany("ReadingLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookHub.Server.Features.Review.Data.Models.Review", b =>
                {
                    b.HasOne("BookHub.Server.Features.Book.Data.Models.Book", "Book")
                        .WithMany("Reviews")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "Creator")
                        .WithMany("Reviews")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("BookHub.Server.Features.Review.Data.Models.Vote", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "Creator")
                        .WithMany("Votes")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Features.Review.Data.Models.Review", "Review")
                        .WithMany("Votes")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("BookHub.Server.Features.UserProfile.Data.Models.UserProfile", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("BookHub.Server.Features.UserProfile.Data.Models.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BookHub.Server.Features.Identity.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookHub.Server.Features.Authors.Data.Models.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("BookHub.Server.Features.Authors.Data.Models.Nationality", b =>
                {
                    b.Navigation("Authors");
                });

            modelBuilder.Entity("BookHub.Server.Features.Book.Data.Models.Book", b =>
                {
                    b.Navigation("BooksGenres");

                    b.Navigation("ReadingLists");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("BookHub.Server.Features.Chat.Data.Models.Chat", b =>
                {
                    b.Navigation("ChatsUsers");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("BookHub.Server.Features.Genre.Data.Models.Genre", b =>
                {
                    b.Navigation("BooksGenres");
                });

            modelBuilder.Entity("BookHub.Server.Features.Identity.Data.Models.User", b =>
                {
                    b.Navigation("Authors");

                    b.Navigation("Books");

                    b.Navigation("ChatsCreated");

                    b.Navigation("ChatsUsers");

                    b.Navigation("Profile");

                    b.Navigation("ReadingLists");

                    b.Navigation("Reviews");

                    b.Navigation("SentChatMessages");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("BookHub.Server.Features.Review.Data.Models.Review", b =>
                {
                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
