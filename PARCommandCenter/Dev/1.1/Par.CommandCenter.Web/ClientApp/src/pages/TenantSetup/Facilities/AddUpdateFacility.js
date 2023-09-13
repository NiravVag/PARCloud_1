import React, { useEffect, useState } from 'react';

import Select from "react-select";

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



const AddUpdateFacility = (props) => {
    
    const [facilityNameRequiredState, setFacilityNameRequiredState] = useState(true);
    const [facilityName, setFacilityName] = useState((props.facility) ? props.facility.name : "");

    const [timeZoneRequiredState, setTimeZoneRequiredState] = useState(true);    
    const [timeZoneSelected, setTimeZoneSelected] = useState();

    const [address, setAddress] = useState((props.facility) ? props.facility.addressLine1 : "");
    const [city, setCity] = useState((props.facility) ? props.facility.city : "");

    const [stateSelected, setStateSelected] = useState();    

    const [zipCode, setZipCode] = useState((props.facility) ? props.facility.postalCode : "");


    const [stateOptions, setStateOptions] = useState([]);
    const [timeZoneOptions, setTimeZoneOptions] = useState([]);

    const [isLoading, setIsLoading] = useState(true);

    const [vpnConnectionSelected, setVPNConnectionSelected] = useState();
    const [vpnConnectionOptions, setVPNConnectionOptions] = useState([]);


   

    useEffect(() => {
        const stateUrl = "api/Lookup/GetStates";
        const timeZonesUrl = "api/Lookup/GetTimeZones";
        const availableVPNConnectionUrl = "api/HealthCheck/GetFacilityVPNConnections";

        const fetchData = async (url) => {
            setIsLoading(true);

            try {  
                const response = await fetch(url);

                if (response.status === 204) {
                    if (url.indexOf("States") > -1) {
                        setStateOptions([]);
                    } else if (url.indexOf("TimeZones") > -1) {
                        setTimeZoneOptions([]);
                    } else if (url.indexOf("VPNConnections") > -1) {
                        setVPNConnectionOptions([]);
                    }
                } else {
                    const data = await response.json();
                    if (url.indexOf("States") > -1) {  
                        let stateOptions = data.states.map(t =>  ({ value: t.id, label: t.name }));
                        stateOptions = [{ value: -1, label: "Select" }, ...stateOptions];
                        
                        if (props.facility && props.facility.state) {
                            let selectedState = stateOptions.find((o) => o.value === props.facility.state.id);
                            setStateSelected(selectedState);
                        }
                        setStateOptions(stateOptions);                       
                    } else if (url.indexOf("TimeZone") > -1) {
                        let timeZoneOptions = data.timeZones.map(t => ({ value: t.id, label: t.name }));
                        timeZoneOptions = [{ value: -1, label: "Select" }, ...timeZoneOptions];
                        if (props.facility && props.facility) {
                            let selectedTimeZone = timeZoneOptions.find((o) => o.value === props.facility.timeZoneId);
                            setTimeZoneSelected(selectedTimeZone);
                        }
                        setTimeZoneOptions(timeZoneOptions);
                    } else if (url.indexOf("VPNConnections") > -1) {
                        let vpnConnectionOptions = data.vpnConnections.map(vpn => {

                            let option = { value: vpn.id, label: vpn.connectionName };
                            if (props.facility && props.facility.vpnConnectionName && props.facility.vpnConnectionName.length > 0) {
                                if (props.facility.vpnConnectionName.toLowerCase() == vpn.connectionName.toLowerCase()) {
                                    setVPNConnectionSelected(option)
                                }
                            }
                            
                            return option;
                        });

                        vpnConnectionOptions = [{ value: -1, label: "Select" }, ...vpnConnectionOptions];                        
                        setVPNConnectionOptions(vpnConnectionOptions);
                    }
                } 
                
            } catch (error) {                
                throw error;
            }

            setIsLoading(false);
        };

        fetchData(stateUrl);
        fetchData(timeZonesUrl);

        if (props.facility && props.facility.id > 0) {
            fetchData(availableVPNConnectionUrl + "?tenantId=" + props.facility.tenantId + "&facilityId=" + props.facility.id);
        }
        else {
            fetchData(availableVPNConnectionUrl + "?tenantId=" + props.tenantId);
        }
    }, []);

    const handleSaveRequest = async () => {        
        try {

            let facility = {
                facilityId: (props.facility) ? props.facility.id : undefined,
                tenantId: props.tenantId,
                name: facilityName,
                timeZoneId: (timeZoneSelected) ? timeZoneSelected.value : undefined,
                vpnConnectionName: (vpnConnectionSelected && vpnConnectionSelected.value != -1) ? vpnConnectionSelected.label : undefined,
                addressLine1: address,
                city: city,
                stateId: (stateSelected && stateSelected.value >= 1 ) ? stateSelected.value : undefined,                
                postalCode: zipCode,
            };

            const url = "api/Facility/Upsert";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify(facility)
                });

            if (response.status === 400) {
                /* Map the validation errors to IErrors */
                let responseBody = await response.json();
                //const errors: any[];
                Object.keys(responseBody).map((key) => {
                    // For ASP.NET core, the field names are in title case - so convert to camel case
                    const fieldName = key.charAt(0).toLowerCase() + key.substring(1);
                    //errors[fieldName] = responseBody[key];
                });
            } else if (response.status === 200) {
                let responseBody = await response.json();

                props.onAddComplete();

                facility.id = responseBody;              
                return response.ok;
            }

        } catch (e) {
            console.log(e);
        }
    };

    return (
        <Container fluid>
            <Form>
            <Row>
                <Col md="12">
                   
                        <Row>
                            <Col>
                                <Form.Group
                                    className={
                                        facilityNameRequiredState ? "has-success" : "has-error"
                                    }
                                >
                                <label>
                                    Name <span className="star">*</span>
                                </label>
                                    <Form.Control
                                        name="name"
                                        type="text"
                                        value={facilityName}
                                        onChange={(e) => {
                                            setFacilityName(e.target.value);
                                            if (isRequired(e.target.value)) {
                                                setFacilityNameRequiredState(true);
                                            } else {
                                                setFacilityNameRequiredState(false);
                                            }
                                        }}
                                    ></Form.Control>
                                    {facilityNameRequiredState ? null : (
                                        <label className="error">
                                            This field is required.
                                        </label>
                                    )}
                                        </Form.Group>
                            </Col>
                        </Row>
                        <Row>
                            <Col>
                                <Form.Group
                                    className={
                                        timeZoneRequiredState ? "has-success" : "has-error"
                                    }
                                >
                                    <label>
                                        Time zone <span className="star">*</span>
                                    </label>
                                    <Select
                                        className="react-select primary"
                                        classNamePrefix="react-select"
                                        name="timeZone"
                                        value={timeZoneSelected}
                                        onChange={(value) => {
                                            setTimeZoneSelected(value)
                                            if (isRequired(value.label)) {
                                                setTimeZoneRequiredState(true);
                                            } else {
                                                setTimeZoneRequiredState(false);
                                            }
                                        }
                                        }
                                        options={timeZoneOptions}
                                        placeholder="Select"
                                    />
                                    {timeZoneRequiredState ? null : (
                                        <label className="error">
                                            This field is required.
                                        </label>
                                    )}
                                        </Form.Group>
                            </Col>
                        </Row>                       
                        <Row>
                            <Col>
                                <Form.Group>
                                    <label>
                                        VPN Connection Name
                                    </label>
                                    <Select
                                        className="react-select primary"
                                        classNamePrefix="react-select"
                                        name="vpnConnectionName"
                                        value={vpnConnectionSelected}
                                        onChange={(value) => {
                                            setVPNConnectionSelected(value);
                                        }
                                        }
                                        options={vpnConnectionOptions}
                                        placeholder="Select"
                                    />
                                    <label>
                                        The VPN connection facility association is used for one tenant at a time. Suppose you were looking for a VPN connection name that is unavailable. It's not available due to the association for another tenant.
                                    </label>
                                    
                                </Form.Group>
                            </Col>
                        </Row>
                        <Row>                        
                            <Col>
                                <Form.Group>
                                    <label>
                                        Address
                                    </label>
                                    <Form.Control
                                        name="address"
                                        type="text"
                                        value={address}
                                        onChange={(e) => {
                                            setAddress(e.target.value)
                                        }}
                                    ></Form.Control>
                                </Form.Group>
                            </Col>
                        </Row>                        
                        <Row className="d-flex justify-content-between">
                            <Col md="4">
                                <Form.Group>
                                    <label>
                                        City
                                    </label>

                                    <Form.Control
                                        name="city"
                                        type="text"
                                        value={city}
                                        onChange={(e) => {
                                            setCity(e.target.value)
                                        }}
                                    ></Form.Control>
                                </Form.Group>
                            </Col>
                            <Col md="4">
                                <Form.Group>
                                    <label>
                                        State
                                    </label>
                                    <Select
                                        className="react-select primary"
                                        classNamePrefix="react-select"
                                        name="state"
                                        value={stateSelected}
                                        onChange={(value) =>
                                            setStateSelected(value)}
                                        options={stateOptions}                                        
                                        placeholder="Select"
                                    />
                                </Form.Group>
                            </Col>
                            <Col md="4">
                                <Form.Group>
                                    <label>
                                        Zip Code
                                    </label>
                                    <Form.Control  
                                        name="zipCode"
                                        type="text"
                                        value={zipCode}
                                        onChange={(e) => {
                                            setZipCode(e.target.value)
                                        }}
                                    ></Form.Control>
                                </Form.Group>
                            </Col>
                        </Row>                            
                   
                </Col>                
            </Row>
            <Row className="mt-3">
                <Col md="12" className="d-flex justify-content-end">
                   
                    <Button className="mr-1" variant="danger"
                        onClick={props.onCancel}>
                        Cancel
                      </Button>
                    <Button className="mr-1" variant="primary"
                            onClick={() => {
                                let valid = true;
                                if (!facilityNameRequiredState || !isRequired(facilityName)) {
                                    setFacilityNameRequiredState(false);  
                                    valid = false;
                                } else {
                                    setFacilityNameRequiredState(true);                                    
                                }
                                if (!timeZoneRequiredState || !isRequired(timeZoneSelected)) {
                                    setTimeZoneRequiredState(false);
                                    valid = false;
                                } else {
                                    setTimeZoneRequiredState(true);                                  
                                }

                                if (valid) {
                                    handleSaveRequest();
                                }

                        }}
                    >
                            Save
                      </Button>
                  
                </Col>
                </Row>
            </Form>
        </Container>
    );
}

export default AddUpdateFacility;