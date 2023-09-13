import React, { useState, useEffect } from 'react';

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



const ErrorFilter = (props) => {

    const [criticalOutagesOnly, setCriticalOutagesOnly] = useState(props.selected);

    const handleCancelErrorsOnly = (event) => {
        props.onCancel();
        setCriticalOutagesOnly(false);
    }

    let errorsOnlyFilterContent;

    if (criticalOutagesOnly) {
        errorsOnlyFilterContent = (
            <Button className="btn-round btn-wd mr-1 btn-filter" variant="default">
                <span className="font-weight-bold" >Critical Outages Only</span>
                <span className="font-weight-bold" onClick={handleCancelErrorsOnly}>
                    <i className="pl-2 fas fa-lg fa-times"></i>
                </span>
            </Button>
        );
    }

    return (
        <>
            {errorsOnlyFilterContent}
        </>
    );
}

export default ErrorFilter