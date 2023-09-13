import React, { useState, useEffect } from 'react';
import { Redirect, useLocation } from 'react-router-dom'

// react component used to create charts


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
    Spinner
} from "react-bootstrap";

import TenantList from "./../TenantList";

import "./SelectTenants.scss";

import { useAppState } from "./../../../state";


function SelectTenantsPage(props) {

    const [tenants, setTenants] = useState();

    const [isSaveLoading, setIsSaveLoading] = useState(false);
    const [saveSucceed, setSaveSucceed] = useState(false);

    const [displayedTenants, setDisplayedTenants] = useState();

    const [selectedTenants, setSelectedTenants] = useState([]);
    const [errorMessage, setErrorMessage] = useState();
    const [cardCSSClass, setCardCSSClasses] = useState("");

    const [redirect, setRedirect] = useState(false);

    const location = useLocation();

    const { user, app, onAppStateRefresh } = useAppState();

    let pageTitleContent;

    useEffect(() => {      
        fetch('api/Tenant/GetAll')
            .then(response => response.json())
            .then(data => {
                let selectedTenants = [];
                // mark user tenant's selected
                let tenants = data.tenants.map((tenant, i) => {
                    tenant.selected = false;
                    if (user.tenantIds.includes(tenant.id)) {
                        tenant.selected = true;
                        // Add the selected to the selected tenant array.
                        selectedTenants.push(tenant.id);
                    }
                    return tenant
                });

                setSelectedTenants(selectedTenants);


                setTenants(tenants);
            });
    }, []);    

    const handleTenantToggleChange = (e) => {
        const tenantId = e.currentTarget.getAttribute("id");
        let userTenants = [...tenants];
        let userSelectedTenants = selectedTenants;
        let found = false;

        for (var i = 0; i < userSelectedTenants.length; i++) {
            if (userSelectedTenants[i] == tenantId) {
                userSelectedTenants.splice(i, 1);
                found = true;
                break;
            }
        }

        if (!found) {
            userSelectedTenants.push(parseInt(tenantId));
        }

        for (var j = 0; j < userTenants.length; j++) {
            userTenants[j].selected = false;
            if (userSelectedTenants.includes(userTenants[j].id)) {
                userTenants[j].selected = true;;
            }
        }      

        setSelectedTenants(userSelectedTenants);
        setTenants(userTenants);
    }

    const onSaveClick = async () => {
        try {
            setSaveSucceed(false);
            if (selectedTenants.length <= 0) {
                setErrorMessage("Please select at least one tenant.");
                setCardCSSClasses("form-group has-error");
                return;
            }
            else {
                setErrorMessage(undefined);
                setCardCSSClasses("");
            }

            setIsSaveLoading(true);

            const url = "api/User/SaveUserTenantSettings";

            const response = await fetch(url,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify(selectedTenants)
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
                setIsSaveLoading(false);
            } else if (response.status === 200) {
                let responseBody = await response.json();

                if (responseBody.successful == true) {
                    
                    setRedirect(true);
                    onAppStateRefresh();
                }

                //await handleRestartRouter();

                //controller.id = responseBody;
                setIsSaveLoading(false);
                setSaveSucceed(true);
                return response.ok;
            }

        } catch (e) {
            console.log(e);
        }

        setIsSaveLoading(false);
    };

   

    let errorMessaggeContent
    if (errorMessage && errorMessage.length > 0) {        
        errorMessaggeContent = (
            <Row>
                <Col className="d-flex justify-content-center ">
                    <label className="error">{errorMessage}</label>
                </Col>
            </Row>
            );
    }

    let succeedMessaggeContent;
    if (saveSucceed) {
        succeedMessaggeContent = (
            <Row>
                <Col className="d-flex justify-content-center ">
                    <label className="success">Setting saved successfully</label>
                </Col>
            </Row>
        );
    }

    if (redirect && location.pathname.indexOf("selectTenant") >= 0) {
        return <Redirect to="/dashboard" />
    }

    let saveButtonContent = (
        <>
            <i className="fa fa-save"></i>
            Save
        </>
    );

    if (isSaveLoading) {
        saveButtonContent = (
           <> 
                                  <Spinner animation="border" size="sm" /> 
              <span> Saving...</span>
        </> 
        );
    }

    if (props.showPageTitle) {
        pageTitleContent = (
            <Card.Header>
                <h3><i className="text-primary fas fa-hospital-user mr-3 "></i> Selected Tenants</h3>
                <p>Select one or more tenant(s) by turning ON and OFF the switch near each option. </p>
                <p>Note: Changing the application tenants will require you to log out from the application, close the browser window and all tabs, then open a new browser window for the new setting to get in effect.</p>
            </Card.Header>
        );
    }
    else {
        pageTitleContent = (
            <Card.Header>                
                <p>Select one or more tenant(s) by turning on and off the switch near each option. </p>
                <p>Note: Changing the application tenants will require you to log out from the application, close the browser window and all tabs, then open a new browser window for the new setting to get in effect.</p>
            </Card.Header>
        );
    }

    return (
        <Container fluid id="selectTenants">
           
           
                <Card className={`table-with-switches bg-white border-0 ${cardCSSClass}`}>
                {pageTitleContent}
                <Card.Body className="p-0 webkit-scroll vertical-overflow">
                    
                    <TenantList tenants={tenants} onTenantToggleChange={handleTenantToggleChange}/>
                    </Card.Body>
                    <Card.Footer>
                        <Row>
                            <Col className="d-flex justify-content-center ">
                            <Button className="mr-1" variant="primary" onClick={!isSaveLoading ? onSaveClick : null}>
                                {saveButtonContent}                               
                                </Button>
                            </Col>
                        </Row>
                    {errorMessaggeContent}
                    {succeedMessaggeContent}
                    </Card.Footer>
                </Card>
            
        </Container>
    );
}

export default SelectTenantsPage;