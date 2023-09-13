import React, { useEffect, useState } from 'react';

import Select from "react-select";

import SweetAlert from 'react-bootstrap-sweetalert';

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

const isRequired = (value) => value !== null && value !== "" && value;
const isNumber = (value) => !isNaN(value) && value !== "";

const AddUpdateController = (props) => {

    const [isLoading, setIsLoading] = useState(false);

    const [alert, setAlert] = useState(null);

    const [saveSucceed, setSaveSucceed] = useState(false);    
    const [updateControllerCompleted, setUpdateControllerCompleted] = useState(false);
    const [updateControllerError, setUpdateControllerError] = useState(false);
    const [restartRouterStarted, setRestartRouterStarted] = useState(false);
    const [saveError, setSaveError] = useState();

    const [routerSelected, setRouterSelected] = useState();
    const [routers, setRouters] = useState([]);
    const [routerOptions, setRouterOptions] = useState([]);
    const [routerRequiredState, setRouterRequiredState] = useState(true);

    const [canAddController, setCanAddController] = useState((props.controller) ? true : false);

    const [portNameRequiredState, setPortNameRequiredState] = useState(true);

    const [portName, setPortName] = useState((props.controller) ? props.controller.portName : "");
    

    const [n2cControllerType, setN2cControllerType] = useState((props.controller) ? ((props.controller.controllerTypeId == 3) ? true : false) : true);
    const [cowServerControllerType, setCowServerControllerType] = useState((props.controller) ? ((props.controller.controllerTypeId == 4) ? true : false) : false);


    const [ipAddress, setIpAddress] = useState((props.controller) ? props.controller.ipAddress : "");

    const [networkPort, setNetworkPort] = useState((props.controller) ? props.controller.networkPort : "");
    const [networkPortNumberState, setNetworkPortNumberState] = useState(true);

    const [macAddress, setMacAddress] = useState((props.controller) ? props.controller.macAddress : "");

    const [parChargeMode, setParChargeMode] = useState((props.controller) ? props.controller.parChargeMode : false);

    const [parChargeBatch, setParChargeBatch] = useState((props.controller) ? props.controller.parChargeBatch : false);

    const [active, setActive] = useState((props.controller) ? props.controller.active : true); 

    useEffect(() => {

        const routerUrl = "api/Router/GetByTenantId/" + props.tenantId;

        const fetchData = async (url) => {
            setIsLoading(true);

            try {
                const response = await fetch(url);

                if (response.status === 204) {
                    if (url.indexOf("Router") > -1) {
                        setRouterOptions([]);
                        setRouters([]);
                    }
                } else {
                    const data = await response.json();
                    if (url.indexOf("Router") > -1) {

                        let routerOptions = data.routers.map(r => ({ value: r.id, label: r.address + "   -   " + r.registeredControllerCount + " Controller" }));
                        routerOptions = [{ value: -1, label: "Select one.." }, ...routerOptions];

                        if (props.controller && props.controller.router) {
                            let selectedRouter = routerOptions.find((o) => o.value === props.controller.router.id);
                            setRouterSelected(selectedRouter);
                        }

                        setRouters(data.routers);
                        setRouterOptions(routerOptions);
                    }
                }

            } catch (error) {
                throw error;
            }

            setIsLoading(false);
        };

        fetchData(routerUrl);
    }, []);


    useEffect(() => {
        getPortName(routerSelected);
    }, [n2cControllerType, cowServerControllerType]);


    const confirmDisconnectControllerMessage = () => {
        setAlert(
            <SweetAlert
                style={{ display: "block", marginTop: "-100px" }}
                title='Are you sure?'
                onConfirm={() => { setActive(!active); hideAlert(); }}
                onCancel={() => hideAlert()}
                confirmBtnBsStyle=" btn-medium btn-danger"
                cancelBtnBsStyle=" btn-medium btn-secondary"
                confirmBtnText="Yes"
                cancelBtnText="Cancel"
                showCancel
            >
                This controller and its dependents will be deactivated, do you want to continue?
            </SweetAlert>
        );
    };

    const hideAlert = () => {
        setAlert(null);
    };

    const handleSaveRequest = async () => {        
        setIsLoading(true);
        setSaveError();

        try {
            let controllerTypeId = 3;
            if (cowServerControllerType) {
                controllerTypeId = 4;
            }

            let controller = {
                controllerId: (props.controller) ? props.controller.id : undefined,
                tenantId: props.tenantId,
                portName: portName,
                routerId: (routerSelected) ? routerSelected.value : undefined,
                controllerTypeId: controllerTypeId,
                ipAddress: ipAddress,
                networkPort: networkPort,
                macAddress: macAddress,
                parChargeBatch: parChargeBatch,
                parChargeMode: parChargeMode,
                active: active,
            };

            const url = "api/Controller/Upsert";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify(controller)
                });

            if (response.status === 400) {
                /* Map the validation errors to IErrors */
                let responseBody = await response.json();
                //const errors: any[];
                Object.keys(responseBody).map((key) => {
                    // For ASP.NET core, the field names are in title case - so convert to camel case
                    const fieldName = key.charAt(0).toLowerCase() + key.substring(1);
                    return fieldName;
                    //errors[fieldName] = responseBody[key];
                });

                setSaveError("Faild to save the controller.");
            } else if (response.status === 200) {
                
                let responseBody = await response.json();

                setSaveSucceed(true);

                controller.id = responseBody;

                
                await timeout(2000);


                setSaveSucceed(false);

                confirmRestartRouterMessage();
                
            } else {
                setSaveError("Faild to save the controller.");
            }

        } catch (e) {
            console.log(e);
            setSaveError("Faild to save the controller.");
        }

        setIsLoading(false);
    };

    const confirmRestartRouterMessage = () => {
        setAlert(
            <SweetAlert
                style={{ display: "block", marginTop: "-100px" }}
                title='Are you sure?'
                onConfirm={async () => {
                    
                    hideAlert();
                    var result = await handleRestartRouter();
                    if (result == true) {                        
                        setUpdateControllerCompleted(true);
                    } else {
                        setUpdateControllerError(true);
                    }

                    await timeout(1000);

                    props.onAddComplete();
                }}
                    
                onCancel={() => {
                    hideAlert();
                    props.onAddComplete();
                }}
                confirmBtnBsStyle=" btn-medium btn-danger"
                cancelBtnBsStyle=" btn-medium btn-secondary"
                confirmBtnText="Yes, restart it!"
                cancelBtnText="Cancel"
                showCancel
            >
                The controller updates require router restart to take effect. It may cause a brief outage while restarting.
            </SweetAlert>
        );
    };    

    const handleRestartRouter = async () => {        
        setRestartRouterStarted(true);

        try {
            let router = routers.find(function (el) {
                return el.id == routerSelected.value;
            });

            const url = "api/Router/Restart";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ address: router.address })
                });

            setRestartRouterStarted(false);

            if (response.status === 400) {
                /* Map the validation errors to IErrors */
                let responseBody = await response.json();
                //const errors: any[];
                Object.keys(responseBody).map((key) => {
                    // For ASP.NET core, the field names are in title case - so convert to camel case
                    const fieldName = key.charAt(0).toLowerCase() + key.substring(1);
                    return fieldName;
                    //errors[fieldName] = responseBody[key];
                });

                setSaveError("Faild to restart the router.");
            } else if (response.status === 200) {
                let responseBody = await response.json();
                return response.ok;
            } else {
                setSaveError("Faild to restart the router.");
            }

        } catch (e) {
            console.log(e);
            setSaveError("Faild to restart the router.");
        }

        setRestartRouterStarted(false);
    }

    const getPortName = async (router) => {        
        if (router && router.value > 0) {
            try {
                const controlerTypeId = (n2cControllerType) ? 3 : ((cowServerControllerType) ? 4 : -1);
                const url = "api/Controller/GetPortName/" + router.value + "/" + controlerTypeId;
                const response = await fetch(url);

                if (response.status === 204) {
                    setPortName("");
                } else {
                    const data = await response.json();
                    setPortName(data.portName);
                }

            } catch (error) {

                throw error;
            }
        }
    }

    const handleRouterSelectionChange = async (option) => {

        let router = routers.find(function (el) {
            return el.id == option.value;
        });

        if (router.registeredControllerCount >= 20) {
            setCanAddController(false);
            setSaveError("You reach the maximum controllers for a Virtual Router");
        }
        else {
            setCanAddController(true);
            setSaveError();
        }

        setRouterSelected(option);
        getPortName(option);
        if (isRequired(option.label)) {
            setRouterRequiredState(true);
        } else {
            setRouterRequiredState(false);
        }
    }

    const timeout = async (ms) => {
        return new Promise(resolve => setTimeout(resolve, ms));
    }


    let saveStatusContent =
        <>
            <Col className="ml-0 pl-0">
                <row>
                    <Col className="d-flex justify-content-between pl-0">
                        {isLoading &&
                            <Col className="ml-0 pl-0 d-flex align-items-center">
                                <Spinner
                                    as="span"
                                    animation="border"
                                    role="status"
                                    variant="primary"
                                    aria-hidden="true"
                                />
                            </Col>

                        }
                        
                        <Col md={12}>
                            {saveSucceed &&
                                <Row>
                                    <span className="font-size-10 m-0 col-auto p-0 text-success">Controller saved successfully</span>
                                </Row>
                            }
                            {restartRouterStarted && 
                                <Row>
                                    <span className="font-size-10 m-0 col-auto p-0">Restarting the router...</span>
                                </Row>
                            }
                            {updateControllerCompleted &&
                                <Row>
                                <span className="font-size-10 m-0 col-auto p-0 text-success">Update router completed successfully</span>
                                </Row>
                            }
                            {updateControllerError &&
                                <Row>
                                    <span className="font-size-10 m-0 col-auto p-0 text-danger">Update router completed with error</span>
                                </Row>
                            }
                            
                        </Col>
                    </Col>
                </row>
            </Col>
        </>;

    let errorSaveStatusContent = (saveError && saveError.length > 0) ?
        (
            <Col className="ml-0 pl-0">
                <row>
                    <Row>
                        <span className="font-size-10 m-0 col-auto p-0 text-danger">{saveError}</span>
                    </Row>
                </row>
            </Col>
        ) :
        undefined;

    return (
        <Container  fluid="md">
            <Form>
                <Row>
                    <Col md="12">
                        <Row className="justify-content-md-center">
                            <Col>
                                <Form.Group>
                                    <Row>                                        
                                        <Col md="6" className="pl-1">
                                            <label>
                                                Controller Type:
                                            </label>
                                            <Form.Check className="form-check-radio">
                                                <Form.Check.Label>
                                                    <Form.Check.Input                                                        
                                                        checked={n2cControllerType}
                                                        defaultValue={3}
                                                        id="n2c"
                                                        name="n2c"
                                                        type="radio"
                                                        onChange={() => {                                                            
                                                            setN2cControllerType(!n2cControllerType);
                                                            setCowServerControllerType(!cowServerControllerType);                                                           
                                                        }}
                                                    ></Form.Check.Input>
                                                    <span className="form-check-sign"></span>
                                                        Cloud Controller (N2C)
                                                </Form.Check.Label>
                                            </Form.Check>
                                            <Form.Check className="form-check-radio">
                                                <Form.Check.Label>
                                                    <Form.Check.Input
                                                        checked={cowServerControllerType}
                                                        defaultValue={4}
                                                        id="cowServer"
                                                        name="cowServer"
                                                        type="radio"
                                                        onChange={() => {                                                            
                                                            setN2cControllerType(!n2cControllerType);
                                                            setCowServerControllerType(!cowServerControllerType);                                                           
                                                        }}
                                                    ></Form.Check.Input>
                                                    <span className="form-check-sign"></span>
                                                    COWServer
                                                </Form.Check.Label>
                                            </Form.Check>
                                        </Col>                                        
                                    </Row>
                                </Form.Group>
                            </Col>
                            <Col className="pl-1">
                                <label className="align-items-center">
                                    Active
                                    <Form.Check
                                        type="switch"
                                        id="active"
                                        name="active"
                                        checked={active}
                                        onChange={() => {
                                            if (active)
                                                confirmDisconnectControllerMessage();
                                            else
                                                setActive(!active);
                                        }}
                                    />
                                </label>

                            </Col>
                        </Row>
                        <Row>
                            <Col md="6">
                                <Form.Group
                                    className={
                                        routerRequiredState ? "has-success" : "has-error"
                                    }
                                >
                                    <label>
                                        Router Address <span className="star">*</span>
                                    </label>
                                    <Select
                                        className="react-select primary"
                                        classNamePrefix="react-select"
                                        name="timeZone"
                                        value={routerSelected}
                                        onChange={(value) =>
                                        {
                                         handleRouterSelectionChange(value)  
                                        }}
                                        options={routerOptions}
                                        placeholder="Select"
                                    />
                                    {routerRequiredState ? null : (
                                        <label className="error">
                                            This field is required.
                                        </label>
                                    )}
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group                                    
                                    className={
                                        portNameRequiredState ? "has-success" : "has-error"
                                    }
                                >
                                    <label>
                                        Port Name <span className="star">*</span>
                                    </label>
                                    <Form.Control
                                        plaintext
                                        tag="p"
                                        defaultValue="-"
                                        value={portName}                                        
                                    >
                                    </Form.Control>                                    
                                </Form.Group>
                            </Col>                           
                        </Row>
                        <Row>
                            <Col>
                                <Form.Group>
                                    <label>
                                        IP Address
                                    </label>
                                    <Form.Control
                                        name="ipAddress"
                                        type="text"
                                        value={ipAddress}
                                        onChange={(e) => {
                                            setIpAddress(e.target.value)
                                        }}

                                        readOnly={!canAddController}
                                    ></Form.Control>
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group className={networkPortNumberState ? "has-success" : "has-error"}>
                                    <label>
                                        Network Port
                                    </label>
                                    <Form.Control
                                        name="networkPort"
                                        type="text"
                                        value={networkPort}
                                        onChange={(e) => {
                                            setNetworkPort(e.target.value)
                                            if (isNumber(e.target.value)) {
                                                setNetworkPortNumberState(true);
                                            } else {
                                                setNetworkPortNumberState(false);
                                            }
                                        }}

                                        readOnly={!canAddController}
                                    ></Form.Control>
                                    {networkPortNumberState ? null : (
                                        <label className="error">
                                            This field is required to be a number.
                                        </label>
                                    )}
                                </Form.Group>
                            </Col>
                        </Row>
                        <Row>
                            <Col>
                                <Form.Group>
                                    <label>
                                        MAC Address
                                    </label>
                                    <Form.Control
                                        name="macAddress"
                                        type="text"
                                        value={macAddress}
                                        onChange={(e) => {
                                            setMacAddress(e.target.value)
                                        }}

                                        readOnly={!canAddController}
                                    ></Form.Control>
                                </Form.Group>
                            </Col>
                            <Col className="d-flex align-items-center">
                                <Row className="pt-4  justify-content-between pr-0">
                                    <Col>
                                        <label className="align-items-center">
                                            Rapid Read
                                            <Form.Check
                                                type="switch"
                                                id="parChargeMode"
                                                name="parChargeMode"
                                                checked={parChargeMode}
                                                onChange={() => {
                                                    /*if (!parChargeBatch) {*/
                                                        setParChargeMode(!parChargeMode)
                                                  /*  }*/
                                                }}

                                                disabled={!canAddController}
                                            />                                            
                                        </label>
                                    </Col>
                                    <Col>
                                        <label className="align-items-center" style={{ whiteSpace: 'nowrap' }}>
                                            Charge Mode
                                            <Form.Check
                                                type="switch"
                                                id="parChargeBatch"
                                                name="parChargeBatch"
                                                checked={parChargeBatch}
                                                onChange={() => {

                                                    //if (!parChargeBatch) {
                                                    //    setParChargeMode(true);
                                                    //}
                                                    setParChargeBatch(!parChargeBatch)
                                                }}

                                                disabled={!canAddController}
                                            />                                           
                                        </label>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>
                        <Row>
                            <Col>
                                <span className="font-size-10 m-0 col-auto p-0 mt-1">20 controllers max is allowed per Virtual Router.<br /> Add new router if you reach this limit.</span>
                            </Col>
                        </Row>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col className="d-flex justify-content-between pr-0">

                        {saveStatusContent}
                        {errorSaveStatusContent}
                        <Col className="d-flex justify-content-end">
                            <Button className="mr-1" variant="secondary"
                                onClick={props.onCancel}>
                                Cancel
                            </Button>
                            <Button className="mr-1" variant="primary" 
                                onClick={() => {
                                    let valid = true;
                                    if (!routerRequiredState || !isRequired(routerSelected)) {
                                        setRouterRequiredState(false);
                                        valid = false;
                                    } else {
                                        setRouterRequiredState(true);
                                    }
                                    if (!portNameRequiredState || !isRequired(portName)) {
                                        setPortNameRequiredState(false);
                                        valid = false;
                                    } else {
                                        setPortNameRequiredState(true);
                                    }
                                    if (networkPort) {
                                        if (!networkPortNumberState || !isNumber(networkPort)) {
                                            setNetworkPortNumberState(false);
                                        } else {
                                            setNetworkPortNumberState(true);
                                        }
                                    }

                                    if (valid) {
                                        handleSaveRequest();
                                    }

                                }}


                                disabled={!canAddController || isLoading}
                            >
                                Save
                            </Button>
                        </Col>
                       

                    </Col>
                </Row>
            </Form>
            {alert}
        </Container>
    );
}

export default AddUpdateController;