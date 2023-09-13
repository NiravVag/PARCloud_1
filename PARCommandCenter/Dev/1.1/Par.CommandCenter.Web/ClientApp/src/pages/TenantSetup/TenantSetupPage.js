////import React, { useState } from 'react';

////// react-bootstrap components
////import {
////    Badge,
////    Button,
////    Card,
////    Form,
////    InputGroup,
////    Navbar,
////    Nav,
////    Table,
////    Container,
////    Row,
////    Col,
////    OverlayTrigger,
////    Tooltip,
////    ListGroup
////} from "react-bootstrap";

////import TenantList from './TenantList/TenantList';
////import TenantSetup from './TenantSetup';

////import './TenantSetup.scss';

////import { useAppState } from "./../../state";

////const TenantSetupPage = (props) => { 

////    const { user, app, onAppStateRefresh } = useAppState();

////    const [selectedTenant, setSelectedTenant] = useState();

////    const [isActive, setActive] = useState(false);

////    let tenantSetup = undefined;

////    const handleSelectTenant = (tenant) => {
////        setSelectedTenant(tenant);
////    }
    
////    if (selectedTenant) {
////        tenantSetup = (<TenantSetup tenant={selectedTenant} onBackButtonClick={() => handleSelectTenant()} />);
////    }
   
       
   
////    return (
////        <>
////            <Container fluid id="tenantSetupPage">
               
////                <Row>

                    
////                    <TenantList user={user} onSelectTenant={(t) => handleSelectTenant(t)} selectedTenant={selectedTenant} />
                  
////                    {tenantSetup}                    
          
                 
                            
////                </Row>    
                
////            </Container>
////        </>
////        );
         
////};

////export default TenantSetupPage;
