<?php

namespace BackendBundle\Controller;

use Sensio\Bundle\FrameworkExtraBundle\Configuration\Route;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\Serializer\Serializer;
use Symfony\Component\Serializer\Encoder\JsonEncoder;
use Symfony\Component\Serializer\Normalizer\GetSetMethodNormalizer;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Response;

/**
 * @Route("/heating")
 */
class HeatingController extends FrameworkController
{
    /**
     * @Route("/overview/{meter}")
     */
    public function overviewAction($meter)
    {
        $yearResults = array();
        
        for($year = 2013; $year < 2017; $year++){
            $sql = "SELECT date, value, name, CASE WHEN lastUsage < 0 THEN 0 ELSE lastUsage END AS lastUsage 
                    FROM(
                    SELECT date, value, name, IFNULL(value - (
                    	SELECT value 
                        FROM tbl_heating_values 
                        WHERE meter_id = t.meter_id AND date < t.date 
                        ORDER BY date DESC LIMIT 0, 1
                    ), 0) AS lastUsage
                    FROM tbl_heating_values t
                    JOIN tbl_heating_meters m ON t.meter_id = m.id
                    WHERE t.meter_id = '{$meter}'
                    AND date BETWEEN '{$year}-01-01' AND '{$year}-12-31'
                    ORDER BY name, date
                    ) resultsTable";
            
            $stmt = $this->getDoctrine()->getEntityManager()->getConnection()->prepare($sql);
            $stmt->execute();
            $yearResults[] = array('year' => $year, 'values' => $stmt->fetchAll());
        }
        
        return parent::Json($yearResults);
    }
    
    /**
     * @Route("/meters")
     */
    public function metersAction()
    {
        $repository = $this->getDoctrine()->getRepository('BackendBundle\Entity\Heating\Meter');
        $meters = $repository->findAll();
        
        return parent::Json($meters);
    }
}