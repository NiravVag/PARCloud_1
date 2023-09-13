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

import "./NotificationSettingPage.scss";

import { useAppState } from "./../../../state";


function NotificationSettingPage(props) {

    const [isLoading, setIsLoading] = useState(false);

    const [tenants, setTenants] = useState();

    const [isSaveLoading, setIsSaveLoading] = useState(false);
    const [saveSucceed, setSaveSucceed] = useState(false);

    const [selectedTenants, setSelectedTenants] = useState([]);

    const [tenantsSetting, setTenantsSetting] = useState([]);

    const [errorMessage, setErrorMessage] = useState();
    const [cardCSSClass, setCardCSSClasses] = useState("");

    useEffect(() => {
        const getAllTenantsURL = "api/Tenant/GetAll";
        const getAppNotificaitonTenantURL = "api/Tenant/GetAllTenantsApplicationSetting/true";

        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setTenants([]);
                    setSelectedTenants([]);
                    setTenantsSetting([]);

                    setIsLoading(false);
                } else {
                    const data = await response.json();

                    if (url == getAllTenantsURL) {
                        if (data.tenants && data.tenants.length > 0) {
                            setTenants(data.tenants);
                        }
                    } else if (url == getAppNotificaitonTenantURL) {
                        if (data.tenantNotificationSettings && data.tenantNotificationSettings.length > 0) {
                            setTenantsSetting(data.tenantNotificationSettings);
                        }
                    }

                    setIsLoading(false);
                    return data;
                }
            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        const getResult = async ()  =>{
            let data1 = await fetchData(getAppNotificaitonTenantURL);
            let data2;
            if (data1.tenantNotificationSettings && data1.tenantNotificationSettings.length > 0) {
                data2 = await fetchData(getAllTenantsURL);
            }
            else {
                return;
            }

            let tenantsSetting = data1.tenantNotificationSettings;
            let tenants = data2.tenants;
           
            let selectedTenants = [];

            if (tenantsSetting && tenantsSetting.length > 0 && tenants && tenants.length > 0) {

                let tenantIds = tenantsSetting.map(t => {
                    return t.tenantId;
                });

                // mark user tenant's selected
                let copyTenants = tenants.map((tenant, i) => {
                    tenant.selected = false;
                    if (tenantIds.includes(tenant.id)) {
                        tenant.selected = true;
                        // Add the selected to the selected tenant array.
                        selectedTenants.push(tenant.id);
                    }
                    return tenant
                });

                setSelectedTenants(selectedTenants);


                setTenants(copyTenants);

            }

            
        }

        getResult();    
        
    }, []);    

    const handleTenantNotificationToggleChange = (e) => {
        let tenantId = e.currentTarget.getAttribute("id");
        tenantId = tenantId.replace("n-", "");
        
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

            const url = "api/Tenant/SaveTenantNotificationSettings";

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
                    
                    //setRedirect(true);
                    //onAppStateRefresh();
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

    //let displayedTenants = tenants;

    //if (tenantsSetting && tenantsSetting.length > 0 && tenants && tenants.length > 0) {
    //    let tenantIds = [];

    //    tenantIds = tenantsSetting.map(t => {
    //        return t.tenantId;
    //    });


    //    displayedTenants = tenants.map((tenant, i) => {
    //        tenant.selected = false;
    //        if (tenantIds.includes(tenant.id)) {
    //            tenant.selected = true;
    //            // Add the selected to the selected tenant array.
    //            // selectedTenants.push(tenant.id);
    //        }
    //        return tenant
    //    });       
    //}
    
    

    return (
        <Container fluid id="selectTenants">
           
           
                <Card className={`table-with-switches bg-white border-0 ${cardCSSClass}`}>
                <Card.Header>
                    <p>You can turn ON and OFF Tenant Zendesk Notification by toggling the switch near each tenant name, then click save to update your selection.</p>
                </Card.Header>
                <Card.Body className="p-0 webkit-scroll vertical-overflow">
                    
                    <TenantList idPrefix="n-" tenants={tenants} onTenantToggleChange={handleTenantNotificationToggleChange} />
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

export default NotificationSettingPage;