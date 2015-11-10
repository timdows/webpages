<?php
namespace BackendBundle\Entity;

class VeraCacheRefresher
{
    private $server = "http://10.0.0.15";
    private $requestString = "/port_3480/data_request?id=status&output_format=json&DeviceNum=%d";
    private $dmString = "/port_3480/data_request?id=lr_dmData&start=%d&stop=%d&channel1=%d";
    private $dmString2 = "/port_3480/data_request?id=lr_dmData&start=%d&stop=%d&channel1=%d&channel2=%d";
    
    public function doRefresh($em){
        //$em = $this->getDoctrine()->getManager();
        
        //Get the doctrine items
        $repository = $em->getRepository('BackendBundle\Entity\VeraCache');
        
        //Last week
        $lastWeekStartLow = $repository->findOneByVariable("lastWeekStartLow");
        $lastWeekStartHigh = $repository->findOneByVariable("lastWeekStartHigh");
        $lastWeekStopLow = $repository->findOneByVariable("lastWeekStopLow");
        $lastWeekStopHigh = $repository->findOneByVariable("lastWeekStopHigh");
        $lastWeekLow = $repository->findOneByVariable("lastWeekLow");
        $lastWeekHigh = $repository->findOneByVariable("lastWeekHigh");
        $lastWeekTotal = $repository->findOneByVariable("lastWeekTotal");
        
        //This week
        $thisWeekStartLow = $repository->findOneByVariable("thisWeekStartLow");
        $thisWeekStartHigh = $repository->findOneByVariable("thisWeekStartHigh");
        $thisWeekStopLow = $repository->findOneByVariable("thisWeekStopLow");
        $thisWeekStopHigh = $repository->findOneByVariable("thisWeekStopHigh");
        $thisWeekLow = $repository->findOneByVariable("thisWeekLow");
        $thisWeekHigh = $repository->findOneByVariable("thisWeekHigh");
        $thisWeekTotal = $repository->findOneByVariable("thisWeekTotal");
        
        //Last Month
        $lastMonthStartLow = $repository->findOneByVariable("lastMonthStartLow");
        $lastMonthStartHigh = $repository->findOneByVariable("lastMonthStartHigh");
        $lastMonthStopLow = $repository->findOneByVariable("lastMonthStopLow");
        $lastMonthStopHigh = $repository->findOneByVariable("lastMonthStopHigh");
        $lastMonthLow = $repository->findOneByVariable("lastMonthLow");
        $lastMonthHigh = $repository->findOneByVariable("lastMonthHigh");
        $lastMonthTotal = $repository->findOneByVariable("lastMonthTotal");
        
        //This Month
        $thisMonthStartLow = $repository->findOneByVariable("thisMonthStartLow");
        $thisMonthStartHigh = $repository->findOneByVariable("thisMonthStartHigh");
        $thisMonthStopLow = $repository->findOneByVariable("thisMonthStopLow");
        $thisMonthStopHigh = $repository->findOneByVariable("thisMonthStopHigh");
        $thisMonthLow = $repository->findOneByVariable("thisMonthLow");
        $thisMonthHigh = $repository->findOneByVariable("thisMonthHigh");
        $thisMonthTotal = $repository->findOneByVariable("thisMonthTotal");
        
        //Get the new week values
        $resultsLastWeekStart = $this->getMultipleValuesByTime(20, 22, strtotime("-2 Monday", strtotime("tomorrow")));
        $resultsLastWeekStop = $this->getMultipleValuesByTime(20, 22, strtotime("last sunday", strtotime("tomorrow")));
        $resultsThisWeekStart = $this->getMultipleValuesByTime(20, 22, strtotime("last monday", strtotime("tomorrow")));
        $resultsThisNow = $this->getMultipleValuesByTime(20, 22, time());
        
        //Set last week
        $lastWeekStartLow->setValueAndUrl($resultsLastWeekStart[0]);
        $lastWeekStartHigh->setValueAndUrl($resultsLastWeekStart[1]);
        $lastWeekStopLow->setValueAndUrl($resultsLastWeekStop[0]);
        $lastWeekStopHigh->setValueAndUrl($resultsLastWeekStop[1]);
        $lastWeekLow->setValue($lastWeekStopLow->getValue() - $lastWeekStartLow->getValue());
        $lastWeekHigh->setValue($lastWeekStopHigh->getValue() - $lastWeekStartHigh->getValue());
        $lastWeekTotal->setValue($lastWeekLow->getValue() + $lastWeekHigh->getValue());
        
        $em->flush();
        
        //Set this week
        $thisWeekStartLow->setValueAndUrl($resultsThisWeekStart[0]);
        $thisWeekStartHigh->setValueAndUrl($resultsThisWeekStart[1]);
        $thisWeekStopLow->setValueAndUrl($resultsThisNow[0]);
        $thisWeekStopHigh->setValueAndUrl($resultsThisNow[1]);
        $thisWeekLow->setValue($thisWeekStopLow->getValue() - $thisWeekStartLow->getValue());
        $thisWeekHigh->setValue($thisWeekStopHigh->getValue() - $thisWeekStartHigh->getValue());
        $thisWeekTotal->setValue($thisWeekLow->getValue() + $thisWeekHigh->getValue());
        
        $em->flush();
        
        //Get the new month values
        $resultsLastMonthStart = $this->getMultipleValuesByTime(20, 22, strtotime(date('Y-m-01') . " -1 month"));
        $resultsLastMonthStop = $this->getMultipleValuesByTime(20, 22, strtotime(date('Y-m-t') . " -1 month"));
        $resultsThisMonthStart = $this->getMultipleValuesByTime(20, 22, strtotime(date('Y-m-01')));
        
        //Set last Month
        $lastMonthStartLow->setValueAndUrl($resultsLastMonthStart[0]);
        $lastMonthStartHigh->setValueAndUrl($resultsLastMonthStart[1]);
        $lastMonthStopLow->setValueAndUrl($resultsLastMonthStop[0]);
        $lastMonthStopHigh->setValueAndUrl($resultsLastMonthStop[1]);
        $lastMonthLow->setValue($lastMonthStopLow->getValue() - $lastMonthStartLow->getValue());
        $lastMonthHigh->setValue($lastMonthStopHigh->getValue() - $lastMonthStartHigh->getValue());
        $lastMonthTotal->setValue($lastMonthLow->getValue() + $lastMonthHigh->getValue());
        
        $em->flush();
        
        //Set this Month
        $thisMonthStartLow->setValueAndUrl($resultsThisMonthStart[0]);
        $thisMonthStartHigh->setValueAndUrl($resultsThisMonthStart[1]);
        $thisMonthStopLow->setValueAndUrl($resultsThisNow[0]);
        $thisMonthStopHigh->setValueAndUrl($resultsThisNow[1]);
        $thisMonthLow->setValue($thisMonthStopLow->getValue() - $thisMonthStartLow->getValue());
        $thisMonthHigh->setValue($thisMonthStopHigh->getValue() - $thisMonthStartHigh->getValue());
        $thisMonthTotal->setValue($thisMonthLow->getValue() + $thisMonthHigh->getValue());
        
        $em->flush();
    }

    private function getMultipleValuesByTime($channel1, $channel2, $time){
        $url = $this->server . sprintf($this->dmString2, $time, $time, $channel1, $channel2);
        echo $url . "\n";
        $rawResponse = json_decode(file_get_contents($url));
        
        $valueChannel1 = new Value();
        $valueChannel1->channel = $channel1;
        $valueChannel1->url = $url;
        $valueChannel1->value = $rawResponse->series[0]->data[0][1];
        
        $valueChannel2 = new Value();
        $valueChannel2->channel = $channel1;
        $valueChannel2->url = $url;
        $valueChannel2->value = $rawResponse->series[1]->data[0][1];
        
        return array(
            $valueChannel1,
            $valueChannel2
        );
    }    
}

class Value
{
     public $channel;
     public $url;
     public $value;
}

   