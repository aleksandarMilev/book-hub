export default function About() {
    return (
        <section className="about_section layout_padding-bottom">
            <div className="container">
                <div className="row">
                    <div className="col-lg-5 col-md-6">
                        <div className="detail-box">
                            <h2>About us</h2>
                            <p>
                                There are many variations of passages of Lorem Ipsum available, but the majority
                                have suffered alteration in some form, by injected humour, or randomised. There are
                                many variations of passages of Lorem Ipsum available, but the majority have suffered
                                alteration in some form, by injected humour, or randomised.
                            </p>
                            <a href="">Read More</a>
                        </div>
                    </div>
                    <div className="col-lg-7 col-md-6">
                        <div className="img-box">
                            <img src="public/images/about-img.jpg" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}
