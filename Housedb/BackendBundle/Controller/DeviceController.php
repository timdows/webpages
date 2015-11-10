<?php

namespace BackendBundle\Controller;

use Sensio\Bundle\FrameworkExtraBundle\Configuration\Route;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;

/**
 * @Route("/api/device")
 */
class DeviceController extends FrameworkController
{
    /**
     * @Route("/details")
     */
    public function detailsAction()
    {
        $repository = $this->getDoctrine()->getRepository('BackendBundle\Entity\Device');
        $readingsFromDb = $repository->findAll();
        
        return parent::Json($readingsFromDb);
    }
}