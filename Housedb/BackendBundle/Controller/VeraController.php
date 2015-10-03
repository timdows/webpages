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
use BackendBundle\ClientModels\SevenSegment;
use BackendBundle\Entity\VeraCache;
use BackendBundle\Entity\VeraCacheRefresher;

/**
 * @Route("/vera")
 */
class VeraController extends Controller
{
    private $server = "http://10.0.0.15";
    private $requestString = "/port_3480/data_request?id=status&output_format=json&DeviceNum=%d";
    private $dmString = "/port_3480/data_request?id=lr_dmData&start=%d&stop=%d&channel1=%d";
    private $dmString2 = "/port_3480/data_request?id=lr_dmData&start=%d&stop=%d&channel1=%d&channel2=%d";
    
    /**
     * @Route("/7segment")
     */
    public function sevenSegmentAction()
    {
        $clientModel = new SevenSegment();
        
        //Get current usage in watt
        $url = $this->server . sprintf($this->requestString, 38);
        $rawResponse = json_decode(file_get_contents($url));
        $clientModel->watt = $rawResponse->Device_Num_38->states[0]->value;
        
        //Get historic usage in kWh
        $repository = $this->getDoctrine()->getRepository('BackendBundle\Entity\VeraCache');
//         $lastWeekStartLow = $repository->findOneByVariable("lastWeekStartLow");
//         $clientModel->lastWeekStartLow = $lastWeekStartLow->getValue();
//         $clientModel->lastWeekStartHigh = $repository->findOneByVariable("lastWeekStartHigh")->getValue();
//         $clientModel->lastWeekStopLow = $repository->findOneByVariable("lastWeekStopLow")->getValue();
//         $clientModel->lastWeekStopHigh = $repository->findOneByVariable("lastWeekStopHigh")->getValue();
//         $clientModel->lastWeekLow = $repository->findOneByVariable("lastWeekLow")->getValue();
//         $clientModel->lastWeekHigh = $repository->findOneByVariable("lastWeekHigh")->getValue();
        $clientModel->lastWeekTotal = $repository->findOneByVariable("lastWeekTotal")->getValue();
        
//         $clientModel->thisWeekStartLow = $repository->findOneByVariable("thisWeekStartLow")->getValue();
//         $clientModel->thisWeekStopLow = $repository->findOneByVariable("thisWeekStopLow")->getValue();
//         $clientModel->thisWeekLow = $repository->findOneByVariable("thisWeekLow")->getValue();
//         $clientModel->thisWeekStartHigh = $repository->findOneByVariable("thisWeekStartHigh")->getValue();
//         $clientModel->thisWeekStopHigh = $repository->findOneByVariable("thisWeekStopHigh")->getValue();
//         $clientModel->thisWeekHigh = $repository->findOneByVariable("thisWeekHigh")->getValue();
        $clientModel->thisWeekTotal = $repository->findOneByVariable("thisWeekTotal")->getValue();
        
        $clientModel->lastMonthTotal = $repository->findOneByVariable("lastMonthTotal")->getValue();
        $clientModel->thisMonthTotal = $repository->findOneByVariable("thisMonthTotal")->getValue();
        
        $serializer = new Serializer(array(new GetSetMethodNormalizer()), array('json' => new JsonEncoder()));
        $json = $serializer->serialize($clientModel, 'json');
    
        $response = new Response($json);
        $response->headers->set('Content-Type', 'application/json');
    
        return $response;
    }
    
    /**
     * @Route("/refreshcaches")
     */
    public function refreshCaches(){
        $refresher = new VeraCacheRefresher();
        $refresher->doRefresh($this->getDoctrine()->getManager());
    }
    
    private function getValueByTime($channel, $time){
        $url = $this->server . sprintf($this->dmString, $time, $time, $channel);
        $rawResponse = json_decode(file_get_contents($url));
        
        return $rawResponse->series[0]->data[0][1];
    }
}
