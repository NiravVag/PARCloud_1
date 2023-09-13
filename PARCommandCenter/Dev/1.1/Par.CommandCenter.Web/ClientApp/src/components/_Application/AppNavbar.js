import React from "react";

import { Link, useLocation } from "react-router-dom";

import logo from './../../styles/images/ParCommandLogo.png';

// react-bootstrap components
import {
    Badge,
    Button,
    ButtonGroup,
    Card,
    Dropdown,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Pagination,
    Container,
    Row,
    Col,
    Collapse,
} from "react-bootstrap";


const getPageName = (routes, location) => {
    for (let i = 0; i < routes.length; i++) {
        if (location.pathname === routes[i].path) {
            return routes[i].name;
        }
    }  
};

function AppNavbar(props) {
    const [collapseOpen, setCollapseOpen] = React.useState(false);

    const pageTitle = getPageName(props.routes, props.location);


    return (
        <>
            <Navbar expand="lg" className="par-navbar">
                <Container fluid className="p-0">
                    <div className="navbar-wrapper w-100">
                    <div className="w-100 row justify-content-between align-items-center">
                        <div className="col-6 d-flex align-items-center">
                       
                            <a>
                                <img className="pl-3 image-200" src={logo} />
                        </a>
                        {
                            !props.logoOnly && 

                            
                                <div className="navbar-minimize ml-4">
                                    <Button
                                        className="btn-fill btn-round btn-icon d-none d-lg-block bg-white border-0 "
                                        variant="dark"
                                        onClick={() => document.body.classList.toggle("sidebar-mini")}
                                    >
                                        <i className="icon-light fa-2x fas fa-bars visible-on-sidebar-regular"></i>
                                        <i className="icon-light fa-2x fas fa-bars visible-on-sidebar-mini"></i>
                                    </Button>
                                    <Button
                                        className="btn-fill btn-round btn-icon d-block d-lg-none bg-white border-0"
                                        variant="dark"
                                        onClick={() =>
                                            document.documentElement.classList.toggle("nav-open")
                                        }
                                    >
                                        <i className="icon-light fa-2x fas fa-bars visible-on-sidebar-regular"></i>
                                        <i className="icon-light fa-2x fas fa-bars visible-on-sidebar-mini"></i>
                                    </Button>
                                </div>
                            
                        } 
                    </div>
                    {
                        !props.logoOnly &&

                        <div className="col-4 d-flex align-items-center">


                            <div id="menuNameDiv">
                                <span className="navbar-page-title">{pageTitle}</span>
                            </div>
                        </div>
                    }
                       
                      
                            <div className=" col-1 col-sm-auto ml-auto navbar-nav"><Dropdown id="settingsMenu" as={Nav.Item}>
                                <Link to="/setting">
                                <Button
                                    className="btn-fill btn-round btn-icon d-none d-lg-block bg-white border-0 "
                                    variant="dark"
                                    
                                >
                                    <i className="icon-light fa-lg fas fa-cog icon-light visible-on-sidebar-regular"></i>
                                    <i className="icon-light fa-lg fas fa-cog icon-light visible-on-sidebar-mini"></i>
                                    </Button>
                                </Link>
                                <Button
                                    className="btn-fill btn-round btn-icon d-block d-lg-none bg-white border-0"
                                    variant="dark"
                                    onClick={() =>
                                        document.documentElement.classList.toggle("nav-open")
                                    }
                                >
                                    <i className="icon-light fa-lg fas fa-cog icon-light visible-on-sidebar-regular"></i>
                                    <i className="icon-light fa-lg fas fa-cog icon-light visible-on-sidebar-mini"></i>
                                </Button>

                                    {/*<Dropdown.Toggle*/}
                                    {/*    as={Nav.Link}*/}
                                    {/*    id="dropdown-41471887333"*/}
                                    {/*    variant="default"*/}
                                    {/*    className=" "*/}
                                    {/*>*/}
                                    {/*    <i className="fa-sm fas fa-cog icon-light"></i>*/}
                                    {/*</Dropdown.Toggle>*/}
                                    {/*<Dropdown.Menu*/}
                                    {/*    alignRight*/}
                                    {/*    aria-labelledby="navbarDropdownMenuLink"*/}
                                    {/*>*/}

                                    {/*    <Dropdown.Item*/}
                                    {/*        href="#ParCommand"*/}
                                    {/*        onClick={(e) => e.preventDefault()}*/}
                                    {/*    >*/}
                                    {/*        <i className="fas fa-tools"></i>*/}
                                    {/*        Settings*/}
                                    {/*    </Dropdown.Item>*/}
                                    {/*    <div className="divider"></div>*/}
                                        
                  {/*                      <Dropdown.Item*/}
                  {/*                          className="text-danger"*/}
                  {/*                          href="#ParCommand"*/}
                  {/*                          onClick={(e) => e.preventDefault()}*/}
                  {/*                      >*/}
                  {/*                          <i className="fas fa-power-off"></i>*/}
                  {/*  Log out*/}
                  {/*</Dropdown.Item>*/}
                                   {/* </Dropdown.Menu>*/}
                                </Dropdown>
                          
                          </div>
                    </div>  
                   

                    </div>
                </Container>
            </Navbar>

        </>
    );
}

export default AppNavbar;
