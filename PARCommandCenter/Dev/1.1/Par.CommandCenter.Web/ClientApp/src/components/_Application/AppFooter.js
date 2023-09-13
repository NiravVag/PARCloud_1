import React from "react";

// react-bootstrap components
import {   
    Container,   
} from "react-bootstrap";

function AppFooter() {
    return (
        <>
            <footer className="footer">
                <Container fluid className="pl-4 ml-2">
                    <nav>
                        <ul className="footer-menu">                       
                        </ul>                      
                    </nav>
                </Container>
            </footer>
        </>
    );
}

export default AppFooter;
