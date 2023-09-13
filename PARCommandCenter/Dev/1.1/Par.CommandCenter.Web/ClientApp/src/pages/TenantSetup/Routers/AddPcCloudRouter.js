import savedGif from '../../../styles/images/ParSaved.gif';
import React, {useState } from 'react';

import {    
    Button,    
    Form,   
    Row,
    Col,
} from "react-bootstrap";

import "./AddPcCloudRouter.scss";

const AddPcCloudRouter = (props) => {

    const [computerNameRequiredState, setComputerNameRequiredState] = useState(true);
    const [computerName, setComputerName] = useState("");

    const isRequired = (value) => value !== null && value !== "" && value;

    const [step1IsLoading, setStep1IsLoading] = useState(false);
    const [step1Completed, setStep1Completed] = useState(false);

    const [step1Error, setStep1Error] = useState(undefined);

    const [step2IsLoading, setStep2IsLoading] = useState(false);
    const [step2Completed, setStep2Completed] = useState(false);
    const [step2Error, setStep2Error] = useState(undefined);


    const [routerAddress, setRouterAddress] = useState("");
    const [serviceName, setServiceName] = useState("");
    const [serviceDisplayName, setServiceDisplayName] = useState("");

    const [hideStartButton, setHideStartButton] = useState(false);

    const [newRouter, setNewRouter] = useState();

    const [addNewRouterCompleted, setAddNewRouterCompleted] = useState(false);

    const [showRouterInfo, setShowRouterInfo] = useState(true);

    const [routerAddressIsValid, setRouterAddressIsValid] = useState(true);

    let status1Content = (step1IsLoading) ? <span className="loaderFind"></span> : undefined;
    let status1ContentStep = (step1IsLoading) ? <strong className="loader-circle"></strong> : <span>1</span>;
    let status2Content = (step2IsLoading) ? <span className="loaderFind"></span> : undefined;
    let status2ContentStep = (step2IsLoading) ? <strong className="loader-circle"></strong> : <span>2</span>;


    let finishedContent, routerInfoContent;
    let finishedContentStep = <span>3</span>;

    const handleStartAddRouterClick = async (e) => {
        setStep1Error(undefined);

        if (computerName & computerName.length <= 0) {
            setStep1Error("You must provide a computer name")
            return;
        }

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

        if ((serviceName && serviceName.length > 0) || (serviceDisplayName && serviceDisplayName.length > 0)) {
            if (routerAddress)
            {
                if (routerAddress.length <= 0) {
                    setStep1Error("You must provide a router address.");
                }
            }
            else
                setStep1Error("You must provide a router address.");
        }

        if (!handleRouterAddressValidation(routerAddress)) {
            setStep1Error("The router address is not valid");
            return;
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
                    body: JSON.stringify({ tenantId: props.tenantId, address: routerAddress, serviceName: serviceName, serviceDisplayName: serviceDisplayName, isPcRouter: true, computerName: computerName})
                });


            if (response.status === 400) {
                let responseBody = await response.json();
                setStep1IsLoading(false);
                setStep1Error("An error occured during registration.");
            } else if (response.status === 200) {
                let responseBody = await response.json();
                let myRouter = {
                    routerId: responseBody.routerId,
                    routerAddress: responseBody.routerAddress,
                }
                setNewRouter(myRouter);
                setTimeout(() => {
                    setStep1IsLoading(false);
                    setStep1Completed(true);

                    // Add new cloud router.
                    createNewCloudRouter(myRouter);
                }, 3000);

                return myRouter;

            } else {
                setStep1Error("An error occured during registration.");
                setStep1IsLoading(false);
            }

        } catch (e) {
            console.log(e);
            setStep1Error("An error occured during registration.");
            setStep1IsLoading(false);
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

                let myRouter = router;
                myRouter.azureIoTDeviceId = responseBody.deviceId;
                setNewRouter(myRouter);

                setTimeout(() => {
                    setStep2IsLoading(false);
                    setStep2Completed(true);

                    addNewRouterSuccessful(myRouter);
                }, 3000);

                return myRouter;
            } else {
                setStep2IsLoading(false);
                setStep2Error("An error occured during creating the cloud router.");
            }

        } catch (e) {
            console.log(e);
            setStep2Error("An error occured during creating the cloud router.");
        }
    }

    const addNewRouterSuccessful = async (router) => {
        if (router.isRunning) {
            setAddNewRouterCompleted(true);
        }
        props.onAddRouterComplete();
    }

    const handleRouterAddressValidation = (address) => {         
        const regex1 = new RegExp('^[A-F 0-9]*$');
        const regex2 = new RegExp('^([A-F0-9]{12})CF$');

        if (address && address.length > 0) {
            setRouterAddressIsValid(false);
            if (address.length < 14) {
                if (regex1.test(address)) {
                    setRouterAddress(address);
                    setRouterAddressIsValid(false);
                    return false;
                }
            }
            else {
                if (regex2.test(address)) {
                    setRouterAddress(address);
                    setRouterAddressIsValid(true);
                    return true;
                }
            }
        }
        else {
            setRouterAddress(address);
            setRouterAddressIsValid(true);
            return true;
        }
    }

    if (showRouterInfo) {
        routerInfoContent = (
            <>
                <Row className="ml-0">
                    <Col>
                        <Form.Group
                            className={routerAddressIsValid ? "has-success" : "has-warning"}
                            >
                            <label>
                                Router Address
                            </label>
                            <Form.Control
                                name="routerAddress"
                                type="text"
                                value={routerAddress}
                                onChange={(e) => {
                                    handleRouterAddressValidation(e.target.value);
                                                             
                                }}
                            >
                            </Form.Control>
                            {!routerAddressIsValid ? null : (
                                <label className="error">
                                    This field is valid.
                                </label>
                            )}
                        </Form.Group>
                        
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
                <Row className="ml-0">
                    <Col>
                        <label className="label-small">
                            <span className="time-zone text-right">An alphanumeric router address must contain 14 uppercase letters from A-F and 0-9 and must end with "CF"</span>
                        </label>
                    </Col>
                </Row>
                {/*<Row className="mt-3">*/}
                {/*    <Col md="3" >*/}
                {/*        <Button className="btn-round mr-1" size="default" variant="secondary" onClick={() => setShowRouterInfo(false)}>Cancel</Button>*/}
                {/*    </Col>*/}
                {/*</Row>*/}
            </>
        );
    }

    let newRouterContent = (
        <>
            <Row className="mb-3">
                <Col md="6">
                    <Form.Group
                        className={
                            computerNameRequiredState ? "has-success" : "has-error"
                        }
                    >
                        <label>
                            Computer Name <span className="star">*</span>
                        </label>
                        <Form.Control
                            name="routerAddress"
                            type="text"
                            value={computerName}
                            onChange={(e) => {
                                setComputerName(e.target.value);
                                if (isRequired(e.target.value)) {
                                    setComputerNameRequiredState(true);
                                } else {
                                    setComputerNameRequiredState(false);
                                }
                            }}
                        >
                        </Form.Control>
                        {computerNameRequiredState ? null : (
                            <label className="error">
                                This field is required.
                            </label>
                        )}
                    </Form.Group>

                </Col>
            </Row>
            <Row>
                <h5 className="ml-3 text-muted">
                    Begin by clicking the start button to auto-generate a router address and service info.  {/* <a className="btn-link" onClick={() => setShowRouterInfo(true)}>Click to Enter Router Info</a>*/}
                </h5>

                {routerInfoContent}
            </Row >
        </>
    );

    let finalServiceContent = undefined;


    if (step1Completed) {
        status1Content = <span className="text-primary"></span>
        status1ContentStep = <img className="gifImagePos" src={savedGif} />
        newRouterContent = <Row>
            <small className="ml-3 text-muted">New router address <strong className="font-weight-bold text-dark">{newRouter.routerAddress}</strong> has been generated</small>
        </Row>
    }

    if (step2Completed) {
        status2ContentStep = <img className="gifImagePos" src={savedGif} />
        status2Content = <span></span>

        finalServiceContent = <Row>

            <small className="ml-3 text-muted">You may close the window at anytime and come back later.</small>
        </Row>;
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

    let startButtonContent = (hideStartButton) ?
        <Button className="btn-md" variant="secondary">
            <span className="btn-label"></span>
            In Progress...
        </Button>
        :

        <Button className="btn-md" variant="primary" 
            onClick={(e) => {
                let valid = true;
                if (!computerNameRequiredState || !isRequired(computerName)) {
                    setComputerNameRequiredState(false);
                    valid = false;
                }
                
                if (valid) {                   
                    handleStartAddRouterClick(e) 
                }
               
            }}
        >
            <span className="btn-label"></span>
            Start
        </Button>
        ;
    

    return (
        <div id="add-pc-cloud-router">
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

export default AddPcCloudRouter;