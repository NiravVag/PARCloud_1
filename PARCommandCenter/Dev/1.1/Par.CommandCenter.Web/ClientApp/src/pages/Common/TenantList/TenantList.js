import React, { useState, useEffect } from 'react';

// react-bootstrap components
import {
    Badge,
    ButtonGroup,
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


import SearchBar from './../../../components/Common/SearchBar';
import './TenantList.scss';


const TenantList = (props) => {
    
    const [testTenantsChecked, setTestTenantsChecked] = useState(false);
    const [allTenantsChecked, setAllTenantsChecked] = useState(false);
    const [tenants, setTenants] = useState();
    const [displayedTenants, setDisplayedTenants] = useState([]);   

    useEffect(() => {
        fetch('api/Tenant/GetByIds', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                tenantIds: props.user.tenantIds,
                optionalField: props.optionalField,
            })
        })
        .then(response => response.json())
        .then(data => {                
            setTenants(data.tenants);                
            filterTenants("nonTestOnly", data.tenants);
        });
    }, []);

    useEffect(() => {
        if (tenants) {           
            if (allTenantsChecked) {
                filterTenants("showAll", tenants);
            } else if (!allTenantsChecked) {
                filterTenants("nonTestOnly", tenants);
            }
        }
    }, [allTenantsChecked]);

    useEffect(() => {
        if (tenants) {
            if (testTenantsChecked) {
                filterTenants("testOnly", tenants);
            } else if (!allTenantsChecked) {
                filterTenants("nonTestOnly", tenants);
            }
        }
    }, [testTenantsChecked]);
    

    const filterTenants = (filter, tenants) => {
        let filteredTenants = [];
        switch(filter) {
            case "showAll":
                filteredTenants = tenants;
                break;
            case "nonTestOnly":
                filteredTenants = tenants.filter(t => {
                    if (t.isTest == false) {
                        return t;
                    }
                });
                break;
            case "testOnly":
                filteredTenants = tenants.filter(t => {
                    if (t.isTest == true) {
                        return t;
                    }
                });
                break;            
        }

        setDisplayedTenants(filteredTenants);
    }

    const handleTenantClick = (e) => {
        for (const item of tenants) {
            if (item.id == e.currentTarget.getAttribute("data-rb-event-key")) {              
                props.onSelectTenant(item);
                break;
            }
        }
    }

    const handleSearchRequest = (searchText) => {
        let searchedTenants = [];

        if (searchText.length <= 0) {
            setDisplayedTenants([]);
        } else {
            searchedTenants = tenants.filter(t => {
                if (t.searchField.search(searchText.toLowerCase()) !== -1) {
                    return t;
                }
            });

            setDisplayedTenants(searchedTenants);
        }
    }

    let tenantItems = [];
    if (displayedTenants && displayedTenants.length > 0) {
        tenantItems = displayedTenants.map((tenant, index) => {
            let testIcon = (tenant.isTest) ? (<div></div>) : undefined;            
            const itemContent = (
                 <div className="d-flex">                    
                    <div className="d-flex justify-content-center list-item-icon">
                        <i className="text-secondary fas fa-map-marker-alt align-self-top pt-2" />
                    </div>
                    <div className="w-100 p-1">
                        <div className="d-flex justify-content-between">
                            <span className="font-weight-bold">{tenant.name}</span>
                            <span className="time-zone text-right">{tenant.timeZone.name}</span>
                        </div>
                        <div className="d-flex justify-content-between">
                            <span className="time-zone font-size-10 m-0 col-auto p-0 mt-1">Order Box: {tenant.orderBoxPercentage}%</span>
                            <span className="time-zone font-size-10 m-0 col-auto p-0 mt-1 ml-1">{tenant.scalesCount} Scales</span>
                        </div>
                        <div className="d-flex  justify-content-between">
                            <div>
                                <span className="time-zone font-size-10 m-0 col-auto p-0 mt-1">Par Mobile Allow Remember Me:</span>
                                <span className="time-zone font-size-10 m-0 col-auto p-0 mt-1 ml-1">{(tenant.parMobileAllowRememberMe) ? <i className="text-success fas fa-lock"></i> : <i className="fas fa-unlock text-danger"></i>}</span>
                            </div>
                            {testIcon}
                        </div>
                    </div>
                </div>
            );

            if (props.selectedTenant && props.selectedTenant.id > 0 && tenant.id === props.selectedTenant.id) {
                return (
                    <ListGroup.Item key={index} eventKey={tenant.id} as="li" active onClick={(event) => { handleTenantClick(event); }}>
                        {itemContent}
                    </ListGroup.Item>
                );
            }
            else {
                return (
                    <ListGroup.Item key={index} eventKey={tenant.id} as="li" action onClick={(event) => { handleTenantClick(event); }}>
                        {itemContent}
                    </ListGroup.Item>
                );
            }
            
        });
    }

    let contentCssClasses;
    
    let footer;
    if (props.optionalField) {
        if (props.optionalField == "ScalesCount" && displayedTenants.length > 0) {
            let totalCount = 0;
            for (const element of displayedTenants) {
               totalCount += element.scalesCount;
            }

            footer = (
                <Card.Footer className="text-muted pb-0 pt-0">
                    <div className="d-flex justify-content-between">
                        <span className="font-size-10 col-auto p-0">{totalCount} Total Scales</span>
                        <span className="font-size-10 col-auto p-0">{displayedTenants.length} Tenants</span>
                    </div>

                </Card.Footer>
            );
                
        }
    }

    return (
        <Col md="3" id="tenantList" className={contentCssClasses}>
            <Card id="tenants-card" className="border-0">
                <Card.Body className="overflow-hidden">

                    <SearchBar onSearch={handleSearchRequest} />

                    <div className="tabsTenant mt-2">
                        <label className="tabTenant">
                            <input onChange={(event) => {
                                setAllTenantsChecked(!allTenantsChecked);
                                if (testTenantsChecked == true)
                                {
                                    setTestTenantsChecked(false);
                                }                                
                            }}
                            type="checkbox" name="tabTenant-input" className="tabTenant-input" checked={allTenantsChecked} />
                            <div className="tabTenant-box">Show All</div>
                        </label>
                        <label className="tabTenant" >
                            <input onChange={(event) => {
                                setTestTenantsChecked(!testTenantsChecked);

                                if (allTenantsChecked == true) {
                                    setAllTenantsChecked(false);
                                }                                
                               
                            }} type="checkbox" name="tabTenant-input" className="tabTenant-input" checked={testTenantsChecked}/>
                            <div className="tabTenant-box">Show Test Only</div>
                         </label>
    
                     </div>
                    <ListGroup variant="flush" className="vertical-overflow">
                        {tenantItems}
                    </ListGroup>
                </Card.Body>
                {footer}
            </Card>
        </Col>
    );
}

export default TenantList;