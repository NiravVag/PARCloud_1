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

const PingController = (props) => {

    const [pingResult, setPingResult] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    let initialMsg = "Ping address (" + props.ipAddress + ")" + " has started ...";

    const handleStartPing = async () => {

        if (props.ipAddress.length <= 0) {
            setIsLoading(false);
            setPingResult("\n ERROR: The IP Address is Empty");

            return;
        }

        if (props.tenantId < 0) {
            setIsLoading(false);
            setPingResult("\n ERROR: The tenant Id is Empty");

            return;
        }

        setIsLoading(true);
        setPingResult(initialMsg + "\nWaiting reply");

        try {
            const url = "api/Controller/PingCloudController";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ address: props.ipAddress, networkPort: props.networkPort, tenantId: props.tenantId })
                });



            if (response.status === 400) {
                let responseBody = await response.json();
                setIsLoading(false);
                setPingResult(initialMsg + "\n ERROR: An error occured.");
            } else if (response.status === 200) {
                let responseBody = await response.json();
                setIsLoading(false);
                if (responseBody.success) {
                    if (responseBody.pingResponse.pingSucceeded) {
                        setPingResult(
                            initialMsg
                            + "\n" + "Ping Completed."
                            + "\n" + responseBody.pingResponse.message
                        );
                        return;
                    }
                }

                setPingResult(
                    initialMsg
                    + "\n" + "Ping controller failed for IP Address (" + props.ipAddress + ")"
                    + "\n" + responseBody.pingResponse.message
                );
            } else {
                setIsLoading(false);
                setPingResult(initialMsg + "\n\nERROR: An error occured.");
            }

        } catch (e) {
            console.log(e);
            setIsLoading(false);
            setPingResult(initialMsg + "\n ERROR: An error occured.");
        }
    }


    let pingButtonContent = (isLoading) ?
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
        : "Ping";

    return (
        <Container fluid>
            <Form>
                <Row>
                    <Col md="12">

                        <Row className="d-flex justify-content-between">
                            <Col md="4">

                                <label>
                                    IP Address
                                </label>
                                <Form.Control
                                    plaintext
                                    tag="p"
                                    defaultValue={props.ipAddress}
                                >
                                </Form.Control>

                            </Col>
                            <Col md="5" className="d-flex justify-content-end p-2">
                                <Button variant="primary" className="mr-2"
                                    onClick={handleStartPing} disabled={isLoading}>
                                    {pingButtonContent}
                                </Button>
                            </Col>
                        </Row>
                        <Row>
                            <Col>
                                <FormControl className="form-control" as="textarea" aria-label="With textarea" rows="5" readOnly defaultValue={pingResult} />
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

export default PingController;