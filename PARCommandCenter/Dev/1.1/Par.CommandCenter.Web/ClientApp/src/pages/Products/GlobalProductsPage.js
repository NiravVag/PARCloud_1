import React, { useState } from 'react';

// react-bootstrap components
import {
    Badge,
    Button,
    Card,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Table,
    Container,
    Row,
    Col,
    OverlayTrigger,
    Tooltip,
    ListGroup
} from "react-bootstrap";

import { useAppState } from "../../state";

import GlobalProductsPanel from "./GlobalProductsPanel";

import './ProductsPage.scss';


const GlobalProductsPage = (props) => {
    const { user, app, onAppStateRefresh } = useAppState();
 
    return (
        <>
            <Container fluid id="globalProductPage">

                <Row>
                    <Col md="12" >                        
                        <div className="bg-white p-2 border-0 card">
                            <GlobalProductsPanel />
                        </div>                       
                    </Col>
                </Row>

            </Container>
        </>
    );
};

export default GlobalProductsPage;
