<?php
namespace BackendBundle\Command;

use Symfony\Bundle\FrameworkBundle\Command\ContainerAwareCommand;
use Symfony\Component\Console\Output\OutputInterface;
use Symfony\Component\Console\Input\InputInterface;
use BackendBundle\Entity\TemperatureDayReading;
use BackendBundle\Entity\PowerKwhDayReading;

class InsertCommand extends ContainerAwareCommand{
    
    protected function configure(){
        $this
        ->setName('importFromDatamine')
        ->setDescription('Imports values from datamine (Temperature and kWh values)')
        ;
    }
    
    protected function execute(InputInterface $input, OutputInterface $output){
        $em = $this->getContainer()->get('doctrine')->getManager();
        $repository = $em->getRepository('BackendBundle\Entity\Device');
        $devices = $repository->findAll();
        
        $date = new \DateTime();
        foreach($devices as $device){
            if($device->getType() == "Temp"){
                $this->doTemperatureImport($device, $date->format("Y"), $date->format("m"));
            }
            elseif($device->getType() == "kWh"){
                $this->doKwhValueImport($device, $date->format("Y"), $date->format("m"));
            }
        }
        
        $output->writeln("Done importing");
    }
    
    private $server = "http://10.0.0.15";
    private $dmString = "/port_3480/data_request?id=lr_dmData&start=%d&stop=%d&channel1=%d";
    
    private function doTemperatureImport($device, $year, $month){
        $em = $this->getContainer()->get('doctrine')->getManager();
        $daysInMonth = cal_days_in_month(CAL_GREGORIAN, $month, $year);
    
        //Check if there are already items in database
        $start = new \DateTime("{$year}-{$month}-01");
        $end = new \DateTime("{$year}-{$month}-{$daysInMonth}");
        
        $query = $em->createQuery('SELECT r
            FROM BackendBundle\Entity\TemperatureDayReading r
            WHERE r.device = :deviceId
            AND r.date BETWEEN :start AND :end'
        );
        
        $query->setParameter('deviceId', $device->getId())
            ->setParameter('start', "{$year}-{$month}-01")
            ->setParameter('end', "{$year}-{$month}-{$daysInMonth}");
        $readings = $query->getResult();
        
        $dateNow = new \DateTime();
        $dateNow->setTime(0, 0, 0);
        
        for($i = 1; $i <= $daysInMonth; $i++){
            $date = new \DateTime();
            $date->setDate($year, $month, $i);
            $date->setTime(0, 0, 0);
        
            //Only import from days in the past
            if($date >= $dateNow)
                continue;
        
            //check if it was not already valid added
            $findReading = array_filter($readings, function($element) use (&$date){
                return $element->getDate() == $date;
            });
            if($findReading != null)
                continue;
        
            $start = $date->getTimestamp();
            $stop = $start + 86400 -1;
            //$url = "{$host}&start={$start}&stop={$stop}&channel1=" . $readingConfig->getVeraConfigId();
            $url = sprintf($this->server . $this->dmString, $start, $stop, $device->getDatamineChannel());
            echo $url . "\n";
            $json = json_decode(file_get_contents($url), true);
        
            $highest = $json['series'][0]['max'];
            $lowest = $json['series'][0]['min'];
    
            //If the highest and lowest are the same
            if($highest == "-9999999999" && $lowest == "9999999999"){
                $highest = $lowest = $json['series'][0]['data'][0][1];
            }
    
            $temperatureDayReading = new TemperatureDayReading();
            $temperatureDayReading->setDevice($device);
            $temperatureDayReading->setDate($date);
            $temperatureDayReading->setIsValid(true);
            $temperatureDayReading->setHighest($highest);
            $temperatureDayReading->setLowest($lowest);
    
            $em->persist($temperatureDayReading);
        }
        
        $em->flush();
    }
    
    private function doKwhValueImport($device, $year, $month){
        $em = $this->getContainer()->get('doctrine')->getManager();
        $daysInMonth = cal_days_in_month(CAL_GREGORIAN, $month, $year);
    
        //Check if there are already items in database
        $start = new \DateTime("{$year}-{$month}-01");
        $end = new \DateTime("{$year}-{$month}-{$daysInMonth}");
        
        $query = $em->createQuery('SELECT r
            FROM BackendBundle\Entity\PowerKwhDayReading r
            WHERE r.device = :deviceId
            AND r.date BETWEEN :start AND :end'
        );
        
        $query->setParameter('deviceId', $device->getId())
            ->setParameter('start', "{$year}-{$month}-01")
            ->setParameter('end', "{$year}-{$month}-{$daysInMonth}");
        $readings = $query->getResult();
        
        $dateNow = new \DateTime();
        $dateNow->setTime(0, 0, 0);
        
        $lastValue = 0;
        for($i = 1; $i <= $daysInMonth; $i++){
            $date = new \DateTime();
            $date->setDate($year, $month, $i);
            $date->setTime(0, 0, 0);
        
            //Only import from days in the past
            if($date >= $dateNow)
                continue;
        
            //check if it was not already valid added
            $findReading = array_filter($readings, function($element) use (&$date){
                return $element->getDate() == $date;
            });
            if($findReading != null)
                continue;
        
            $start = $date->getTimestamp();
            $stop = $start + 86400 -1;

            $url = sprintf($this->server . $this->dmString, $start, $stop, $device->getDatamineChannel());
            echo $url . " (" . $date->format('Y-m-d') . ") ";
            $json = json_decode(file_get_contents($url), true);
        
            $value = $json['series'][0]['min'];
            if($value == 9999999999){
                $value = $json['series'][0]['data'][1][1];
            }
            echo $value . " ";
            
            $valid = false;
            if($lastValue != $value){
                $lastValue = $value;
                $valid = true;
            }
            echo $valid . "\n";       
            
            $powerKwhDayReading = new PowerKwhDayReading();
            $powerKwhDayReading->setDevice($device);
            $powerKwhDayReading->setDate($date);
            $powerKwhDayReading->setIsValid($valid);
            $powerKwhDayReading->setValue($value);
    
            $em->persist($powerKwhDayReading);
        }
        
        $em->flush();
    }
    
    private function getValueByTime($channel, $time){
        $url = $this->server . sprintf($this->dmString, $time, $time, $channel);
        $rawResponse = json_decode(file_get_contents($url));
    
        return $rawResponse->series[0]->data[0][1];
    }
}