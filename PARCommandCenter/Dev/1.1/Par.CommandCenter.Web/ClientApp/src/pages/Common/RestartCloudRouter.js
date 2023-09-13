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

const RestartCloudRouter = (props) => {

    const [restartResult, setRestartResult] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    let initialMsg = "Restart Router (" + props.routerAddress + ")" + " has started ...";

    const handleStartRestart = async () => {

        if (props.routerAddress.length <= 0) {
            setIsLoading(false);
            setRestartResult("\n ERROR: The Router Address is Empty");

            return;
        }

        setIsLoading(true);
        setRestartResult(initialMsg + "\nWaiting reply");

        try {
            const url = "api/router/restart";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ address: props.routerAddress})
                });

            if (response.status === 400) {
                let responseBody = await response.json();
                setIsLoading(false);
                setRestartResult(initialMsg + "\n ERROR: An error occured.");
            } else if (response.status === 200) {
                let responseBody = await response.json();
                setIsLoading(false);
                if (responseBody.success) {
                  
                        setRestartResult(
                            initialMsg
                            + "\n" + "Restart Router Completed."
                        );
                        return;
                   
                }

                setRestartResult(
                    initialMsg
                    + "\n" + "Retart Router failed for Router Address (" + props.routerAddress + ")"                    
                );
            } else {
                setIsLoading(false);
                setRestartResult(initialMsg + "\n\nERROR: An error occured.");
            }

        } catch (e) {
            console.log(e);
            setIsLoading(false);
            setRestartResult(initialMsg + "\n ERROR: An error occured.");
        }
    }


    let restartButtonContent = (isLoading) ?
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
        : "Restart";

    return (
        <Container fluid>
            <Form>
                <Row>
                    <Col md="12">
                       
                        <Row>
                            <Col>
                                <FormControl className="form-control" as="textarea" aria-label="With textarea" rows="5" readOnly defaultValue={restartResult} />
                            </Col>
                        </Row>
                        <Row className="d-flex justify-content-between pt-2">
                            <Col md="4">

                            </Col>
                            <Col md="5" className="d-flex justify-content-end p-2">
                                <Button variant="primary" className="mr-2"
                                    onClick={handleStartRestart} disabled={isLoading}>
                                    {restartButtonContent}
                                </Button>
                            </Col>
                        </Row>
                    </Col>
                </Row>
                {/*<Row className="mt-3">*/}
                {/*    <Col md="12" className="d-flex justify-content-end">*/}

                {/*        <Button variant="danger"*/}
                {/*            onClick={props.onCancel}>*/}
                {/*            Cancel*/}
                {/*        </Button>*/}
                {/*    </Col>*/}
                {/*</Row>*/}
            </Form>
        </Container>
    );
}

export default RestartCloudRouter;