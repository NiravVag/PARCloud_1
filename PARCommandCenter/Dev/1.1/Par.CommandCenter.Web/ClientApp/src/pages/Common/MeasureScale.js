import React, { useEffect, useState } from 'react';

import {
    Badge,
    Button,
    ButtonGroup,
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
    ListGroup,
    Modal,
    FormControl,
    Spinner
} from "react-bootstrap";

const MeasureScale = (props) => {

    const [measureResult, setMeasureResult] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    let initialMsg = "Measure Scale for address (" + props.scale.address + ")" + " has started ...";

    const handleStartMeasure = async () => {

        if (props.scale.address.length <= 0) {
            setIsLoading(false);
            setMeasureResult("\n ERROR: The provided scale address is not correct or empty");
            
            return;
        }

        setIsLoading(true);
        setMeasureResult(initialMsg + "\nWaiting reply");

        try {
            const url = "api/Scale/RequestBinMeasurement/" + props.scale.binId;

            const response = await fetch(url);



            if (response.status === 400) {
                let responseBody = await response.json();
                setIsLoading(false);
                setMeasureResult(initialMsg + "\n ERROR: An error occured.");
            } else if (response.status === 200) {
                let responseBody = await response.json();
               
                setMeasureResult(
                    initialMsg
                        + "\n" + "Measure scale Completed."
                        + "\n" + "Scale weight value: "
                        + ((responseBody.weight) ? responseBody.weight : "-").toString()
                        + "\n" + "Scale quantity value: "
                        + ((responseBody.quantity) ? responseBody.quantity : "-").toString()
                );

                setIsLoading(false);

                //setMeasureResult("\n" + "Measure scale failed for address (" + props.scale.address + ")");
            } else {
                setIsLoading(false);
                setMeasureResult(initialMsg + "\n\nERROR: An error occured.");
            }

        } catch (e) {
            console.log(e);
            setIsLoading(false);
            setMeasureResult(initialMsg + "\n ERROR: An error occured.");
        }
    }
    

    let startButtonContent = (isLoading) ?
        <>
            <Spinner
                as="span"
                animation="grow"
                size="sm"
                role="status"
                aria-hidden="true"
            />
            Loading...
        </>
        : "Start";

    return (
        <Container fluid>
            <Form>
                <Row>
                    <Col md="12">
                        <Row className="d-flex justify-content-between">
                            <Col md="4">
                                <label>
                                    Scale Address
                                </label>
                                <Form.Control
                                    plaintext
                                    tag="p"
                                    defaultValue={props.scale.address}
                                >
                                </Form.Control>
                            </Col>
                            <Col md="5" className="d-flex justify-content-end p-2">
                                <Button variant="primary" className="mr-2"
                                    onClick={handleStartMeasure} disabled={isLoading}>
                                    {startButtonContent}
                                </Button>
                            </Col>
                        </Row>
                        <Row>
                            <Col>
                                <FormControl className="form-control" as="textarea" aria-label="With textarea" rows="5" readOnly defaultValue={measureResult} />
                            </Col>
                        </Row>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col md="12" className="d-flex justify-content-end">

                        <Button variant="danger"
                            onClick={props.onCancel}>
                            Cancel
                        </Button>
                    </Col>
                </Row>
            </Form>
        </Container>
    );
}

export default MeasureScale;