export default function Client() {
    return (
        <section className="client_section">
            <div style={{ textAlign: "center", padding: "20px" }}>
                <h2>What Our Clients Say</h2>
            </div>
            <div style={{ display: "flex", flexWrap: "wrap", justifyContent: "center", gap: "20px" }}>
                <div style={{ width: "300px", border: "1px solid #ddd", padding: "20px", borderRadius: "8px", boxShadow: "0px 0px 10px rgba(0,0,0,0.1)" }}>
                    <div style={{ display: "flex", alignItems: "center", marginBottom: "15px" }}>
                        <div style={{ marginRight: "10px" }}>
                            <img
                                src="public/images/client-1.jpg"
                                alt="Client 1"
                                style={{ width: "60px", height: "60px", borderRadius: "50%" }}
                            />
                        </div>
                        <div>
                            <h6 style={{ margin: "0" }}>Jorch Morik</h6>
                            <div style={{ color: "#f1c40f" }}>
                                ★★★★★
                            </div>
                        </div>
                    </div>
                    <p style={{ fontSize: "14px", color: "#555" }}>
                        Chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum.
                    </p>
                </div>
                <div style={{ width: "300px", border: "1px solid #ddd", padding: "20px", borderRadius: "8px", boxShadow: "0px 0px 10px rgba(0,0,0,0.1)" }}>
                    <div style={{ display: "flex", alignItems: "center", marginBottom: "15px" }}>
                        <div style={{ marginRight: "10px" }}>
                            <img
                                src="public/images/client-2.jpg"
                                alt="Client 2"
                                style={{ width: "60px", height: "60px", borderRadius: "50%" }}
                            />
                        </div>
                        <div>
                            <h6 style={{ margin: "0" }}>Jane Doe</h6> 
                            <div style={{ color: "#f1c40f" }}>
                                ★★★★★
                            </div>
                        </div>
                    </div>
                    <p style={{ fontSize: "14px", color: "#555" }}>
                        Chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum.
                    </p>
                </div>
            </div>
        </section>
    );
}
