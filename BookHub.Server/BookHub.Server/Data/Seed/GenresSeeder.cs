namespace BookHub.Server.Data.Seed
{
    using Models;

    public static class GenresSeeder
    {
        public static Genre[] Seed()
            => new Genre[]
            {
                new()
                {
                    Id = 1,
                    Name = "Horror",
                    Description =
                        "Horror fiction is designed to scare, unsettle, or horrify readers. It explores themes of fear and the unknown, " +
                        "often incorporating supernatural elements like ghosts, monsters, or curses. The genre can also delve into the darker aspects " +
                        "of human psychology, portraying paranoia, obsession, and moral corruption. Subgenres include Gothic horror, psychological horror, " +
                        "and splatterpunk, each offering unique ways to evoke dread. Settings often amplify the tension, ranging from haunted houses to " +
                        "desolate landscapes, while the stories frequently address societal fears and existential questions.",
                    ImageUrl = "https://org-dcmp-staticassets.s3.us-east-1.amazonaws.com/posterimages/13453_1.jpg"
                },
                new()
                {
                    Id = 2,
                    Name = "Science Fiction",
                    Description =
                        "Science fiction explores futuristic, scientific, and technological themes, challenging readers to consider the possibilities and " +
                        "consequences of innovation. These stories often involve space exploration, artificial intelligence, time travel, or parallel " +
                        "universes. Beyond the speculative elements, science fiction frequently tackles ethical dilemmas, societal transformations, and " +
                        "the human condition. Subgenres include cyberpunk, space opera, and hard science fiction, each offering distinct visions of the future. " +
                        "The genre invites readers to imagine the impact of progress and to ponder humanity’s place in the cosmos.",
                    ImageUrl = "https://www.editoreric.com/greatlit/litgraphics/book-spiral-galaxy.jpg"
                },
                new()
                {
                    Id = 3,
                    Name = "Fantasy",
                    Description =
                        "Fantasy stories transport readers to magical realms filled with mythical creatures, enchanted objects, and epic quests. These tales " +
                        "often feature battles between good and evil, drawing upon folklore, mythology, and the human imagination. Characters may wield powerful " +
                        "magic or undertake journeys of self-discovery in richly crafted worlds. Subgenres like high fantasy, urban fantasy, and dark fantasy " +
                        "provide diverse settings and tones, appealing to a wide range of readers. Themes of heroism, destiny, and transformation are central to " +
                        "the genre, offering both escape and inspiration.",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5EcrB6fhai5L3-7Ted6fZgxUjCti0W4avrA&s"
                },
                new()
                {
                    Id = 4,
                    Name = "Mystery",
                    Description =
                        "Mystery fiction is a puzzle-driven genre that engages readers with suspense and intrigue. The narrative typically revolves around solving " +
                        "a crime, uncovering hidden truths, or exposing a web of deceit. Protagonists range from amateur sleuths to seasoned detectives, each " +
                        "navigating clues, red herrings, and unexpected twists. Subgenres such as noir, cozy mysteries, and legal thrillers cater to varied tastes. " +
                        "Mystery stories often delve into human motives and societal dynamics, providing a satisfying journey toward uncovering the truth.",
                    ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/what-is-a-mystery.jpg"
                },
                new()
                {
                    Id = 5,
                    Name = "Romance",
                    Description =
                        "Romance novels celebrate the complexities of love and relationships, weaving stories of passion, connection, and emotional growth. " +
                        "They can be set in diverse contexts, from historical periods to fantastical worlds, and often feature characters overcoming personal or " +
                        "external obstacles to find happiness. Subgenres like contemporary romance, historical romance, and paranormal romance offer unique flavors " +
                        "and settings. The genre emphasizes emotional resonance, with narratives that inspire hope and affirm the power of love.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/36/Hammond-SS10.jpg"
                },
                new()
                {
                    Id = 6,
                    Name = "Thriller",
                    Description =
                        "Thrillers are characterized by their fast-paced, high-stakes plots designed to keep readers on edge. They often involve life-and-death " +
                        "scenarios, sinister conspiracies, or relentless antagonists. The genre thrives on tension and unexpected twists, with protagonists racing " +
                        "against time to prevent disaster. Subgenres like psychological thrillers, spy thrillers, and action thrillers cater to diverse interests. " +
                        "The stories explore themes of survival, justice, and moral ambiguity, delivering an adrenaline-fueled reading experience.",
                    ImageUrl = "https://celadonbooks.com/wp-content/uploads/2019/10/what-is-a-thriller-1024x768.jpg"
                },
                new()
                {
                    Id = 7,
                    Name = "Adventure",
                    Description =
                        "Adventure stories are dynamic tales of action, exploration, and survival. Protagonists often face daunting challenges, traversing " +
                        "uncharted territories or overcoming perilous odds. The genre celebrates courage, resilience, and the human spirit, taking readers on " +
                        "exhilarating journeys. From treasure hunts to epic battles, adventure fiction encompasses diverse settings and narratives. It appeals to " +
                        "those who crave excitement and the thrill of discovery.",
                    ImageUrl = "https://thumbs.dreamstime.com/b/open-book-ship-sailing-waves-concept-reading-adventure-literature-generative-ai-270347849.jpg"
                },
                new()
                {
                    Id = 8,
                    Name = "Historical",
                    Description =
                        "Historical fiction immerses readers in the past, blending factual events with fictional narratives to create vivid portrayals of bygone eras. " +
                        "These stories illuminate the lives, struggles, and triumphs of people from different times, providing insight into cultural, social, and " +
                        "political contexts. Subgenres include historical romance, historical mysteries, and alternate histories, each offering unique perspectives. " +
                        "The genre enriches our understanding of history while engaging us with compelling characters and plots.",
                    ImageUrl = "https://celadonbooks.com/wp-content/uploads/2020/03/Historical-Fiction-scaled.jpg"
                },
                new()
                {
                    Id = 9,
                    Name = "Biography",
                    Description =
                        "Biographies chronicle the lives of real individuals, offering intimate portraits of their experiences, achievements, and legacies. These works " +
                        "range from comprehensive life stories to focused accounts of specific events or periods. Biographies can inspire, inform, and provide deep " +
                        "insight into historical or contemporary figures. Autobiographies and memoirs, subgenres of biography, allow subjects to share their own " +
                        "narratives, adding personal depth to the genre.",
                    ImageUrl = "https://i0.wp.com/uspeakgreek.com/wp-content/uploads/2024/01/biography.webp?fit=780%2C780&ssl=1"
                },
                new()
                {
                    Id = 10,
                    Name = "Self-help",
                    Description =
                        "Self-help books are guides to personal growth, offering practical advice for improving one’s life. Topics range from mental health and " +
                        "relationships to productivity and spiritual fulfillment. The genre emphasizes empowerment, providing readers with strategies and tools for " +
                        "achieving goals and overcoming challenges. Subgenres include motivational literature, mindfulness guides, and career development books, " +
                        "catering to diverse needs and aspirations.",
                    ImageUrl = "https://www.wellnessroadpsychology.com/wp-content/uploads/2024/05/Self-Help.jpg"
                },
                new()
                {
                    Id = 11,
                    Name = "Non-fiction",
                    Description =
                        "Non-fiction encompasses works rooted in factual information, offering insights into real-world topics. It spans memoirs, investigative journalism, " +
                        "essays, and academic studies, covering subjects like history, science, culture, and politics. The genre educates and engages readers, often " +
                        "challenging perceptions and broadening understanding. Non-fiction can be narrative-driven or expository, appealing to those seeking knowledge " +
                        "or a deeper connection to reality.",
                    ImageUrl = "https://pickbestbook.com/wp-content/uploads/2023/06/Nonfiction-Literature-1.png"
                },
                new()
                {
                    Id = 12,
                    Name = "Poetry",
                    Description =
                        "Poetry is a literary form that condenses emotions, thoughts, and imagery into carefully chosen words, often structured with rhythm and meter. " +
                        "It explores universal themes such as love, nature, grief, and introspection, offering readers profound and evocative experiences. " +
                        "From traditional sonnets and haikus to free verse and spoken word, poetry captivates through its ability to articulate the inexpressible, " +
                        "creating deep emotional resonance and intellectual reflection.",
                    ImageUrl = "https://assets.ltkcontent.com/images/9037/examples-of-poetry-genres_7abbbb2796.jpg"
                },
                new()
                {
                    Id = 13,
                    Name = "Drama",
                    Description =
                        "Drama fiction delves into emotional and relational conflicts, portraying the complexities of human interactions and emotions. " +
                        "It emphasizes character development and nuanced storytelling, often exploring themes of love, betrayal, identity, and societal struggles. " +
                        "Drama offers readers a lens into the intricacies of the human experience, whether through tragic, romantic, or morally ambiguous narratives. " +
                        "Its focus on realism and emotional depth creates stories that resonate deeply with audiences.",
                    ImageUrl = "https://basudewacademichub.in/wp-content/uploads/2024/02/drama-literature-solution.png"
                },
                new()
                {
                    Id = 14,
                    Name = "Children's",
                    Description =
                        "Children's literature is crafted to captivate and inspire young readers with imaginative worlds, moral lessons, and relatable characters. " +
                        "These stories often emphasize themes of curiosity, friendship, and bravery, delivering messages of kindness, resilience, and growth. " +
                        "From whimsical picture books to adventurous chapter books, children's fiction nurtures creativity and fosters a lifelong love of reading, " +
                        "helping young minds explore both real and fantastical realms.",
                    ImageUrl = "https://media.vanityfair.com/photos/598888671dc63c45b7b1db6e/master/w_2560%2Cc_limit/MAG-0817-Wild-Things-a.jpg"
                },
                new()
                {
                    Id = 15,
                    Name = "Young Adult",
                    Description =
                        "Young Adult (YA) fiction speaks to the unique experiences and challenges of adolescence, addressing themes such as identity, first love, " +
                        "friendship, and coming of age. These stories often feature relatable protagonists navigating personal growth, societal expectations, and " +
                        "emotional upheaval. Subgenres such as fantasy, dystopian, and contemporary YA provide diverse backdrops for these journeys, resonating with " +
                        "readers through authentic and engaging storytelling that reflects their own struggles and triumphs.",
                    ImageUrl = "https://m.media-amazon.com/images/I/81xRLF1KCAL._AC_UF1000,1000_QL80_.jpg"
                },
                new()
                {
                    Id = 16,
                    Name = "Comedy",
                    Description =
                        "Comedy fiction aims to entertain and delight readers through humor, satire, and absurdity. It uses wit and clever storytelling to highlight " +
                        "human follies, societal quirks, or surreal situations. From lighthearted escapades to biting social commentary, comedy encompasses a range " +
                        "of tones and styles. The genre often brings laughter and joy, offering an escape from the mundane while sometimes delivering thought-provoking " +
                        "messages in the guise of humor.",
                    ImageUrl = "https://mandyevebarnett.com/wp-content/uploads/2017/12/humor.jpg?w=640"
                },
                new()
                {
                    Id = 17,
                    Name = "Graphic Novel",
                    Description =
                        "Graphic novels seamlessly blend visual art and narrative storytelling, using a combination of text and illustrations to convey complex plots and emotions. " +
                        "This versatile format spans a wide array of genres, including superhero tales, memoirs, historical epics, and science fiction. Graphic novels " +
                        "offer an immersive reading experience, appealing to diverse audiences through their ability to convey vivid imagery and intricate storylines " +
                        "that are as impactful as traditional prose.",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSb0THovTlPB_nRl3RY6TsbWD4R2qEC-TQSAg&s"
                },
                new()
                {
                    Id = 18,
                    Name = "Other",
                    Description =
                        "The 'Other' genre serves as a home for unconventional, experimental, or cross-genre works that defy traditional categorization. " +
                        "This category embraces innovation and diversity, welcoming stories that push the boundaries of storytelling, structure, and style. " +
                        "From hybrid narratives to avant-garde experiments, 'Other' offers a platform for unique voices and creative expressions " +
                        "that don’t fit neatly into predefined genres.",
                    ImageUrl = "https://www.98thpercentile.com/hubfs/388x203%20(4).png"
                },
                new()
                {
                    Id = 19,
                    Name = "Dystopian",
                    Description =
                        "Dystopian fiction paints a grim portrait of societies marred by oppression, inequality, or disaster, often set in a future shaped by " +
                        "catastrophic events or authoritarian regimes. These cautionary tales explore themes like survival, rebellion, and the loss of humanity, " +
                        "serving as critiques of political, social, and environmental trends. Subgenres such as post-apocalyptic and cyber-dystopia examine " +
                        "the fragility of civilization and the consequences of unchecked power or technological overreach.",
                    ImageUrl = "https://www.ideology-theory-practice.org/uploads/1/3/5/5/135563566/050_orig.jpg"
                },
                new()
                {
                    Id = 20,
                    Name = "Spirituality",
                    Description =
                        "Spirituality books delve into the deeper questions of existence, faith, and the human soul, offering insights and practices to " +
                        "nurture inner peace and personal growth. They often explore themes of mindfulness, self-awareness, and connection to a higher " +
                        "power or universal energy. From philosophical reflections to practical guides, these works resonate with readers seeking" +
                        "inspiration, understanding, and spiritual fulfillment across diverse traditions and belief systems.",
                    ImageUrl = "https://m.media-amazon.com/images/I/61jxcM3UskL._AC_UF1000,1000_QL80_.jpg"
                }
            };
    }
}
