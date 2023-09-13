import React from "react";

// react-bootstrap components
import {
    Badge,
    Button,
    Card,
    Collapse,
    Form,
    InputGroup,
    Navbar,
    Nav,
    OverlayTrigger,
    Table,
    Tooltip,
    Container,
    Row,
    Col,
} from "react-bootstrap";

function ForbiddenPage() {

    return (
        <>
            <Container fluid>
                <div className="row align-items-center v-100">
                    <div className="error-page">
                    <i className="fas fa-exclamation-triangle text-danger fa-3x pr-5 mr-3"></i>
                    <h1 className="font-weight-500 title">Not Authorized</h1>
                        <div className="text mb-5">It looks as if you are not authorized to enter Command Center. Please contact your administrator.</div>
                    </div>
                    </div>

                <div className="area">
                    <ul className="circles">
                        <li></li>
                        <li></li>
                        <li></li>
                        <li></li>
                        <li></li>
                        <li></li>
                        <li></li>
                        <li></li>
                        <li></li>
                        <li></li>
                    </ul>
                </div>
               
            </Container>
        </>
    )
}




export default ForbiddenPage;






