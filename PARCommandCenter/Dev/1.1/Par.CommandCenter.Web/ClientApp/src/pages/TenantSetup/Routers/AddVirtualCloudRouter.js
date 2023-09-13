import savedGif from '../../../styles/images/ParSaved.gif';
import ReactDOM from 'react-dom';
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

const AddVirtualCloudRouter = (props) => {

    const [step1IsLoading, setStep1IsLoading] = useState(false);
    const [step1Completed, setStep1Completed] = useState(false);
    const [step1CompletedCR, setStep1CompletedCR] = useState(false);
    const [step1Error, setStep1Error] = useState(undefined);

    const [step2IsLoading, setStep2IsLoading] = useState(false);
    const [step2Completed, setStep2Completed] = useState(false);
    const [step2Error, setStep2Error] = useState(undefined);

    const [step3IsLoading, setStep3IsLoading] = useState(false);
    const [step3Completed, setStep3Completed] = useState(false);
    const [step3Error, setStep3Error] = useState(undefined);

    const [routerAddress, setRouterAddress] = useState("");
    const [serviceName, setServiceName] = useState("");
    const [serviceDisplayName, setServiceDisplayName] = useState("");

    const [hideStartButton, setHideStartButton] = useState(false);

    const [newRouter, setNewRouter] = useState();

    const [addNewRouterCompleted, setAddNewRouterCompleted] = useState(false);

    const [showRouterInfo, setShowRouterInfo] = useState(false);

    let status1Content = (step1IsLoading) ? <span className="loaderFind"></span> : undefined;
    let status1ContentStep = (step1IsLoading) ? <strong className="loader-circle"></strong> : <span>1</span>;
    let status2Content = (step2IsLoading) ? <span className="loaderFind"></span> : undefined;
    let status2ContentStep = (step2IsLoading) ? <strong className="loader-circle"></strong> : <span>2</span>;
    let status3Content = (step3IsLoading) ? <span className="loaderFind"></span> : undefined;
    let status3ContentStep = (step3IsLoading) ? <strong className="loader-circle"></strong> : <span>3</span>;
    // let status3ContentStep = (step4IsLoading) ? <strong className="loader-circle"></strong> : <span>3</span>;
    let finishedContent, routerInfoContent;
    let finishedContentStep = (step3IsLoading) ? <span>4</span> : <span>4</span>;

    const handleStartAddRouterClick = async (e) => {
        setStep1Error("");
        // Validate the Router Address and service name        
        if (routerAddress && routerAddress.length > 0) {
            if (!serviceName && serviceName.length <= 0) {
                setStep1Error("You must provide service name")
                return;
            }

            if (!serviceDisplayName && serviceDisplayName.length <= 0) {
                setStep1Error("You must provide service display name");
                return;
            }
        }

        //Register new router
        setHideStartButton(true);
        registerNewRouter();
    }

    const registerNewRouter = async () => {
        setStep1IsLoading(true);

        try {
            const url = "api/Router/Register";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ tenantId: props.tenantId, address: routerAddress, serviceName: serviceName, serviceDisplayName: serviceDisplayName })
                });



            if (response.status === 400) {
                let responseBody = await response.json();
                setStep1IsLoading(false);
                setStep1Error("An error occured during registration.");
            } else if (response.status === 200) {
                let responseBody = await response.json();
                let newRouter = {
                    routerId: responseBody.routerId,
                    routerAddress: responseBody.routerAddress,
                }
                setNewRouter(newRouter);
                setTimeout(() => {
                    setStep1IsLoading(false);
                    setStep1Completed(true);

                    // Add new cloud router.
                    createNewCloudRouter(newRouter);
                }, 3000);

                return newRouter;

            } else {
                setStep1Error("An error occured during registration.");
            }


        } catch (e) {
            console.log(e);
            setStep1Error("An error occured during registration.");
        }
    }

    const createNewCloudRouter = async (router) => {
        setStep2IsLoading(true);

        try {
            const url = "api/Router/Create";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ address: router.routerAddress })
                });


            if (response.status === 400) {
                let responseBody = await response.json();
                setStep2IsLoading(false);
                setStep2Error("An error occured during the Cloud Router Registration.");
            } else if (response.status === 200) {
                let responseBody = await response.json();

                let newRouter = router;
                newRouter.azureIoTDeviceId = responseBody.deviceId;
                setNewRouter(newRouter);

                setTimeout(() => {
                    setStep2IsLoading(false);
                    setStep2Completed(true);

                    startNewRouterService(newRouter);
                }, 3000);

                return newRouter;
            } else {
                setStep2IsLoading(false);
                setStep2Error("An error occured during creating the cloud router.");
            }


        } catch (e) {
            console.log(e);
            setStep2Error("An error occured during creating the cloud router.");
        }
    }

    const startNewRouterService = async (router) => {
        setStep3IsLoading(true);

        try {
            const url = "api/Router/InstallCloudRouterService";

            const response = await fetch(url,
                {
                    method: "put",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ address: router.routerAddress, serviceName: serviceName, serviceDisplayName: serviceDisplayName })
                });


            if (response.status === 400) {
                let responseBody = await response.json();
                setStep3IsLoading(false);
                setStep3Error("An error occured during creating the cloud router.");
            } else if (response.status === 200) {
                let responseBody = await response.json();

                let newRouter = router;
                newRouter.isRunning = responseBody.isRunning;
                setNewRouter(newRouter);

                setTimeout(() => {
                    setStep3IsLoading(false);
                    setStep3Completed(true);

                    addNewRouterSuccessful(newRouter);
                }, 3000);

                return newRouter;
            } else {
                setStep3Error("An error occured during creating the cloud router.");
            }

        } catch (e) {
            console.log(e);
            setStep3Error("An error occured during creating the cloud router.");
        }
    }

    const addNewRouterSuccessful = async (router) => {
        if (router.isRunning) {
            setAddNewRouterCompleted(true);
        }
        props.onAddRouterComplete();
    }

    if (showRouterInfo) {
        routerInfoContent = (
            <>
                <Row>
                    <Col>

                        <label>
                            Router Address
                                    </label>
                        <Form.Control
                            name="routerAddress"
                            type="text"
                            value={routerAddress}
                            onChange={(e) => {
                                setRouterAddress(e.target.value)
                            }}
                        >
                        </Form.Control>

                    </Col>
                    <Col>

                        <label>
                            Service Name
                                    </label>
                        <Form.Control
                            name="serviceName"
                            type="text"
                            value={serviceName}
                            onChange={(e) => {
                                setServiceName(e.target.value)
                            }}
                        ></Form.Control>

                    </Col>
                    <Col>

                        <label>
                            Service Display Name
                                    </label>
                        <Form.Control
                            name="serviceDisplayName"
                            type="text"
                            value={serviceDisplayName}
                            onChange={(e) => {
                                setServiceDisplayName(e.target.value)
                            }}
                        ></Form.Control>

                    </Col>
                </Row>               
                <Row className="mt-3">
                    <Col md="3" >                        
                        <Button className="btn-round mr-1" size="default" variant="secondary" onClick={() => setShowRouterInfo(false)}>Cancel</Button>
                    </Col>                 
                </Row>
            </>
        );
    }

    let newRouterContent = (
        <Row>
            <h5 className="ml-3 text-muted">
                Begin by clicking the start button to auto-generate a router address and service info.   <a className="btn-link" onClick={() => setShowRouterInfo(true)}>Click to Enter Router Info</a>
            </h5>

            {routerInfoContent}
        </Row >

    );

    let newServiceContent = undefined;
    let finalServiceContent = undefined;


    if (step1Completed) {
        status1Content = <span className="text-primary"></span>
        status1ContentStep = <img className="gifImagePos" src={savedGif} />
        newRouterContent = <Row>
            <small className="ml-3 text-muted">New router address <strong className="font-weight-bold text-dark">{newRouter.routerAddress}</strong> has been generated</small>
        </Row>
    }

    let newCloudRouterContent;
    if (step1CompletedCR) {
        status1Content = <span className="text-primary"></span>
        status1ContentStep = <img className="gifImagePos" src={savedGif} />
        newCloudRouterContent = <Row>
            <small className="ml-3 text-muted">New router address <strong className="font-weight-bold text-dark">{newRouter.routerAddress}</strong> has been generated</small>
        </Row>
    }

    if (step2Completed) {
        status2ContentStep = <img className="gifImagePos" src={savedGif} />
        status2Content = <span></span>
        newServiceContent = <Row>
            <small className="ml-3 text-muted">This step will take several minutes, please hold..</small>

        </Row>;

        finalServiceContent = <Row>

            <small className="ml-3 text-muted">You may close the window at anytime and come back later.</small>
        </Row>;
    }

    if (step3Completed) {
        status3Content = <span className="text-primary">New router service has been completed</span>
        status3ContentStep = <img className="gifImagePos" src={savedGif} />
        finishedContentStep = <img className="gifImagePos" src={savedGif} />
    }

    if (addNewRouterCompleted) {
        finishedContent = <span className="text-success">Successful</span>
    }

    if (step1Error) {

        status1Content = <span className="text-danger">{step1Error}</span>
        status1ContentStep = <i className="fas fa-times text-danger"></i>
    }
    if (step2Error) {
        status2Content = <span className="text-danger">{step2Error}</span>
        status2ContentStep = <i className="fas fa-times text-danger"></i>
    }
    if (step3Error) {
        status3Content = <span className="text-danger">{step3Error}</span>
        status3ContentStep = <i className="fas fa-times text-danger"></i>
    }

    let startButtonContent = (hideStartButton) ?
        <Button className="btn-md" variant="secondary">
            <span className="btn-label"></span>
            In Progress...
        </Button>
        :

        <Button className="btn-md" variant="primary" onClick={(e) => handleStartAddRouterClick(e)}>
            <span className="btn-label"></span>
            Start
        </Button>
        ;
    

    return (
        <div>            
            <Row className="justify-content-center">
            <div className="col-1 mr-3">
                <div className="steps-form">
                    <div className="steps-row setup-panel d-flex justify-content-between">
                        <div className="steps-step">
                            <a href="#" type="button"
                                className="btn btn-circle waves-effect ml-0 btn-pink btn-info"
                                data-toggle="tooltip" data-placement="top" title="" data-original-title="Basic Information">

                                {status1ContentStep}

                            </a>
                        </div>
                        <div className="steps-step">
                            <a href="#" type="button" className="btn btn-pink btn-circle waves-effect p-3" data-toggle="tooltip" data-placement="top" title="" data-original-title="Personal Data">

                                {status2ContentStep}

                            </a>
                        </div>
                        <div className="steps-step">
                            <a href="#" type="button" className="btn btn-pink btn-circle waves-effect" data-toggle="tooltip" data-placement="top" title="" data-original-title="Terms and conditions">

                                {status3ContentStep}

                            </a>
                        </div>
                        <div className="steps-step no-height">
                            <a href="#" type="button" className="btn btn-pink btn-circle waves-effect p-3" data-toggle="tooltip" data-placement="top" title="" data-original-title="Finish">
                                {finishedContentStep}
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            <div className="col-9 steps-form">

                <div className="steps-h">
                    <h5 className="font-weight-bold pl-0 ">
                        <strong>Registering new router</strong>
                    </h5>
                    {newRouterContent}
                    {status1Content}
                </div>


                <div className="steps-h">
                    <h5 className="font-weight-bold pl-0 ">
                        <strong>Link router to the cloud</strong>
                    </h5>
                    {status2Content}
                </div>



                <div className="steps-h">

                    <h5 className="font-weight-bold pl-0 ">
                        <strong>Start the new router service</strong>
                    </h5>


                    {status3Content}
                    {newServiceContent}
                </div>


                <div className="steps-h">
                    <h5 className="font-weight-bold pl-0 ">
                        <strong>Finished</strong>
                    </h5>
                    {finalServiceContent}

                    {finishedContent}
                </div>



            </div>
        </Row>


            <Row className="text-center justify-content-md-center row">
                <Col md="auto">
                    {startButtonContent}
                </Col>
            </Row>
        </div>
    );
    
}

export default AddVirtualCloudRouter;